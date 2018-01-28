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
using Utility;

namespace Master
{
    public class MyServerSocket : Utility.IComponent, IDisposable
    {
        private Socket _socket;
        protected event Evento _output;
        private bool _shouldStop;
        private Thread _thread;
        private SynchronizationContext _syncContext; //crea un "luogo"(contesto) di sincronizzazione tra i thread
        private bool _notifyDispose;
        private bool _disposed;

        #region Properties
        public bool NotifyDispose
        {
            get { return _notifyDispose; }
            set { _notifyDispose = value; }
        }
        public bool ShouldStop
        {
            get { if (_disposed) {throw new ObjectDisposedException("");} return _shouldStop; }
            set { if (_disposed) {throw new ObjectDisposedException("");} _shouldStop = value; }
        }
        #endregion

        public MyServerSocket(string local_ip, PortEnum local_port)
        {
            _syncContext = Documento.GetIstance().SyncContext;
            IPHostEntry hostEntry = Dns.GetHostEntry(local_ip);
            IPEndPoint ipe = new IPEndPoint(hostEntry.AddressList[0], (int)local_port);
            _socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(ipe);
            InitMyServerSocket();
        }

        public void InitMyServerSocket()
        {
            _notifyDispose = true;
            _shouldStop = false;
            _disposed = false;
            _socket.Listen(10);
            _thread = new Thread(RunServer);
            _thread.Start();
        }

        #region iComponent
        public void Input(MessageEventArgs messaggio){/*do nothing*/;}

        public event Evento Output
        {
            add { _output += value; }
            remove { _output -= value; }
        }
        #endregion

        public void RunServer()
        {
            if (_disposed) {throw new ObjectDisposedException("");}
            Socket temp = null;
            MySocket mytemp = null;
            try
            {
                while (!_shouldStop)
                {
                    temp = _socket.Accept();
                    mytemp = new MySocket(temp);
                    if (DocumentoMaster.GetIstance().CurrentPartita!= null)
                    {
                        DocumentoMaster.GetIstance().CurrentPartita.BindComponent(mytemp);
                    }
                    else
                    {
                        temp.Close();
                    } 
                }
            }
            catch (ThreadAbortException) { 
                if (!_disposed && _syncContext != null)
                    if (_notifyDispose) _syncContext.Post(SyncOutput, new ServerSocketClosedMessageEventArgs("MyServerSocket chiusa"));
            }
            catch (Exception e) { 
                if (!_disposed && _syncContext != null) 
                    if (_notifyDispose)_syncContext.Post(SyncOutput, new ServerSocketErrorMessageEventArgs("MyServerSocket chiusa")); 
            }
        }
        
        private void SyncOutput(object state)
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
                        if (_socket != null) _socket.Close();
                        if(_thread != null) _thread.Abort();
                    }
                    //dispose unmanaged resources
                    _shouldStop = true;
                    _socket = null;
                    _thread = null;
                    _disposed = true;
                }
            }
        }
    }
}

namespace Utility.Messages
{
    [Serializable]
    public class ServerSocketErrorMessageEventArgs : MessageEventArgs
    {
        public ServerSocketErrorMessageEventArgs(String argomento) : base(argomento) { }
        public ServerSocketErrorMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class ServerSocketClosedMessageEventArgs : MessageEventArgs
    {
        public ServerSocketClosedMessageEventArgs(String argomento) : base(argomento) { }
        public ServerSocketClosedMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}