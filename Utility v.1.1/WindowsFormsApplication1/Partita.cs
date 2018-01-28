using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using System.Threading;
using Utility.Messages;

namespace Utility
{
    public class Partita : Panel
    {
        //Usato per la Reflection
        private BindingFlags searchFor = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
        //specifica quali tipi di metodo cercare via reflection: Statici (or) nonStatici (or) Pubblici (or) nonPubblici (or) di classi figlie o padre

        protected List<IComponent> _components;
        protected FlowLayoutPanel _flowPanel;

        
        protected String _path;

        protected bool _disposed;

        public String Path
        {
            get { if (_disposed) {throw new ObjectDisposedException("");} return _path; }
            set { if (_disposed) {throw new ObjectDisposedException("");} _path = value; }
        }

        public Partita()
        {
            _components= new List<IComponent>();
            _flowPanel = new FlowLayoutPanel();
            
            this.ParentChanged += InitHandler;
            _disposed = false;
        }

        public Partita(string path) : this() { _path = path; }

        public IEnumerable<IComponent> GetComponentsByType(Type type)
        {
            return from component in _components where component.GetType().ToString() == type.ToString() select component;
        }

        private void Adapt(object sender, EventArgs args) 
        {
            foreach (Control c in _flowPanel.Controls)
            {
                if (c is UserControl && c is IVisualizable)
                {
                    if (((IVisualizable)c).WidthType == DimensionType.Percent) c.Width = (c.Parent.Width - 30) * ((IVisualizable)c).DisplayWidth / 100;
                }
                if (c is UserControl && c is IVisualizable)
                {
                    if (((IVisualizable)c).HeightType == DimensionType.Percent) c.Height = (c.Parent.Height - 30) * ((IVisualizable)c).DisplayHeight / 100;
                }
            }
        }
        private void InitHandler(object sender, EventArgs args) { Init(); }
        private void Init()
        {
            this.Dock = DockStyle.Fill;
            _flowPanel.AutoScroll = true;
            _flowPanel.Dock = DockStyle.Fill;
            _flowPanel.WrapContents = true;
            this.Controls.Add(_flowPanel);
            
            this.Resize += Adapt;
        }


        #region BindComponent - Draw
        public void BindComponent(IComponent component)
        {
            if (_disposed) {throw new ObjectDisposedException("");}
            try
            {
                this.GetType().GetMethod("Draw", searchFor, null, new Type[] { component.GetType() }, new ParameterModifier[0]).Invoke(this, new object[] { component });
            }
            catch (NullReferenceException) { NotImplementedDraw(component); }
            catch (Exception ex) { Console.WriteLine("Partita.BindComponent > a Reflection Exception occurred:\n" + ex.InnerException + "\n" + "trying to continue anyways..."); }
        }

        protected void Draw(ChatComune chatComune)
        {
            chatComune.Output += MessageForwarder; 
            _components.Add(chatComune);
            chatComune.Height = chatComune.DisplayHeight;
            chatComune.Anchor = AnchorStyles.Top;
            _flowPanel.Controls.Add(chatComune);
        }
        protected void Draw(GestoreSchede gestoreSchede)
        {
            gestoreSchede.Output += MessageForwarder;
            _components.Add(gestoreSchede);
            gestoreSchede.Anchor = AnchorStyles.Top;
            _flowPanel.Controls.Add(gestoreSchede);
            gestoreSchede.Height = gestoreSchede.DisplayHeight;
        }
        protected void Draw(MySocket mySocket)
        {
            mySocket.Output += SocketForwarder;
            _components.Add(mySocket);
        }


        protected void NotImplementedDraw(IComponent component)
        {
            Console.WriteLine("An error occurred while trying to handle an " + component.GetType() + "");
            Console.WriteLine("\tMaybe an handler for " + component.GetType() + " is still not implemented!");
        }
        #endregion

        #region SocketForwarder

        public void SocketForwarder(Object o, MessageEventArgs e) 
        {
            if (_disposed) throw new ObjectDisposedException("Partita Disposed");
            try
            {
                if (o is IComponent) SocketForwardMessage(o as IComponent, e);
            }
            catch (Exception ex) { _components.Remove(o as IComponent); }
           
        }

        private void SocketForwardMessage(IComponent sender, MessageEventArgs e)
        {
            try
            {
                this.GetType().GetMethod("SocketMessageHandler", searchFor, null, new Type[] { sender.GetType(), e.GetType() }, new ParameterModifier[0]).Invoke(this, new object[] { sender, e });
            }
            catch (Exception ex) { Console.WriteLine("Partita.ForwardMessage > a Reflection Exception occurred:\n" + ex.InnerException + "\n" + "trying to continue anyways..."); }
        }

        protected virtual void SocketMessageHandler(IComponent sender, MessageEventArgs e)
        {
            Console.WriteLine("An error occurred while trying to forward an " + e.GetType() + " message");
            Console.WriteLine("\tMaybe an handler for " + e.GetType() + " is still not implemented!");
            Console.WriteLine("\tHowever, the message recived was: " + e);
        }

        protected virtual void SocketMessageHandler(MySocket sender, SocketErrorMessageEventArgs e)
        {
            sender.Dispose();
            _components.Remove(sender);
        }

        protected virtual void SocketMessageHandler(MySocket sender, SocketClosedMessageEventArgs e)
        {
            sender.Dispose();
            _components.Remove(sender);
        }

        protected virtual void SocketMessageHandler(MySocket sender, ChatComuneMessageEventArgs e)
        {
            foreach (IComponent c in _components) if (c is ChatComune) c.Input(e);
        }
        #endregion

        #region MessageForwarder

        public void MessageForwarder(Object o, MessageEventArgs e)
        {
            if (_disposed) throw new ObjectDisposedException("");
            if(o is IComponent) ForwardMessage(o as IComponent,e); 
        }

        private void ForwardMessage(IComponent sender, MessageEventArgs e)
        {
            try
            {
                this.GetType().GetMethod("MessageHandler", searchFor, null, new Type[] { sender.GetType(), e.GetType() }, new ParameterModifier[0]).Invoke(this, new object[] { sender, e });
            }
            catch (Exception ex) { Console.WriteLine("Partita.ForwardMessage > a Reflection Exception occurred:\n" + ex.InnerException + "\n" + "trying to continue anyways..."); }
        }

        protected virtual void MessageHandler(IComponent sender, MessageEventArgs e)
        {
            Console.WriteLine("An error occurred while trying to forward an " + e.GetType() + " message");
            Console.WriteLine("\tMaybe an handler for " + e.GetType() + " is still not implemented!");
            Console.WriteLine("\tHowever, the message recived was: " + e);
        }

        protected virtual void MessageHandler(IComponent sender, ChatComuneMessageEventArgs e)
        {
            foreach (IComponent c in _components) if (c is MySocket) c.Input(e);
        }

        protected virtual void MessageHandler(IComponent sender, InterpretaMessageEventArgs e)
        {
            foreach (IComponent c in _components) if (c is GestoreSchede) c.Input(e);
        }

        protected virtual void MessageHandler(IComponent sender, ChatCoordinamentoMessageEventArgs e)
        {
            ; //do nothing
        }
        #endregion

        private void InitializeComponent()
        {
            if (_disposed) {throw new ObjectDisposedException("");}
            this.SuspendLayout();
            this.ResumeLayout(false);
        }


        protected override void Dispose(bool disposing)
        {
            lock (this)
            {
                if(!_disposed)
                {
                    if (disposing)
                    {
                        //dispose managed resources
                        foreach (IComponent c in _components)
                        {
                            if (c != null && c is IDisposable)
                            {
                                if (c is MySocket) (c as MySocket).NotifyDispose = false;
                                (c as IDisposable).Dispose();
                            }
                        }
                        if(_flowPanel!=null)_flowPanel.Dispose();
                    }
                    //dispose unmanaged resources
                    _flowPanel = null;
                    _components.Clear();
                    _components = null;
                    _path = null;
                    _disposed = true;
                    }
                }
            }
    }
}
