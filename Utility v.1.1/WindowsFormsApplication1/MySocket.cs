using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.ComponentModel;
using Utility.Messages;

namespace Utility
{
    public class MySocket : IComponent, IDisposable
    {
        private Socket _socket;
        private NetworkStream _stream;
        protected event Evento _output;
        private IFormatter _formatter;
        private bool _shouldStop;
        private Thread _thread;
        private string _remoteAddress;
        private SynchronizationContext _syncContext; //crea un "luogo"(contesto) di sincronizzazione tra i thread
        private bool _notifyDispose;
        private bool _disposed;

        #region Properties
        public string RemoteAddress
        {
            get { return _remoteAddress; }
        }
        public bool NotifyDispose
        {
            get { return _notifyDispose; }
            set { _notifyDispose = value; }
        }

        public bool ShouldStop
        {
            get { return _shouldStop; }
            set { _shouldStop = value; }
        }
        #endregion

        public MySocket(string remote_ip, PortEnum remote_port)
        {

            _syncContext = Documento.GetIstance().SyncContext;
            _remoteAddress = remote_ip + ":" + remote_port;
            IPHostEntry hostEntry = Dns.GetHostEntry(remote_ip);
            foreach (IPAddress remote_address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(remote_address, (int)remote_port);
                _socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _remoteAddress = remote_address.ToString();               
                try { _socket.Connect(ipe); }
                catch { continue; }
                if (_socket.Connected) break;
            }

            if (!_socket.Connected) throw new SocketException();
            else InitMySocket();
        }

        public MySocket(Socket socket)
        {
            _syncContext = Documento.GetIstance().SyncContext;
            _socket = socket;
            _remoteAddress = (_socket.RemoteEndPoint as IPEndPoint).Address.ToString() + " " + (_socket.RemoteEndPoint as IPEndPoint).Port.ToString();
            InitMySocket();
        }

        private void InitMySocket()
        {
            _notifyDispose = true;
            _shouldStop = false;
            _disposed = false;
            _formatter = new BinaryFormatter();
            if (!_socket.Connected) { if(_notifyDispose)_output(this, new SocketErrorMessageEventArgs("Socket: nessuna connessione trovata")); }
            else
            {
                _stream = new NetworkStream(_socket);
                _thread = new Thread(Run);
                _thread.Start();
            }
           
        }

        #region iComponent
        public void Input(MessageEventArgs messaggio)
        {
            if (!_socket.Connected) _output(this, new SocketErrorMessageEventArgs("Socket: nessuna connessione trovata"));
            else _formatter.Serialize(_stream, messaggio);
        }

        public event Evento Output
        {
            add { _output += value; }
            remove { _output -= value; }
        }
        #endregion

        public Boolean isConnected()
        {
            if (_disposed) {throw new ObjectDisposedException("");}
            return (_socket != null || _socket.Connected || _stream != null);
        }

        private void Run()
        {
            if (_disposed) { throw new ObjectDisposedException(""); }
            MessageEventArgs messaggio = null;
            try
            {
                while (!_shouldStop)
                {
                    messaggio = (MessageEventArgs)_formatter.Deserialize(_stream);
                    if (messaggio != null && _syncContext != null) _syncContext.Post(SyncOutput, messaggio);

                    messaggio = null;
                }
                _shouldStop = false;
            }
            catch (ThreadAbortException) 
            {
                if (_notifyDispose) _syncContext.Post(SyncOutput, new SocketClosedMessageEventArgs(_remoteAddress));
                _remoteAddress = null;
            }
            catch (Exception e) 
            {
                if(!_disposed && _syncContext != null) if (_notifyDispose)_syncContext.Post(SyncOutput, new SocketErrorMessageEventArgs(_remoteAddress));             
            }
        }

        private void SyncOutput (object state)
        {
            if (state is MessageEventArgs) _output(this, (MessageEventArgs)state);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        //dispose managed resources
                        if (_stream != null) _stream.Close();
                        if (_socket != null) _socket.Close();
                        if (_thread != null) _thread.Abort();
                    }
                    //dispose unmanaged resources
                    _shouldStop = true;
                    _stream = null;
                    _socket = null;
                    _thread = null;
                    _formatter = null;
                    _disposed = true;
                }
            }
        }

    }
}

namespace Utility.Messages
{
    [Serializable]
    public class SocketErrorMessageEventArgs : MessageEventArgs
    {
        public SocketErrorMessageEventArgs(String argomento) : base(argomento) { }
        public SocketErrorMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class SocketClosedMessageEventArgs : MessageEventArgs
    {
        public SocketClosedMessageEventArgs(String argomento) : base(argomento) { }
        public SocketClosedMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}