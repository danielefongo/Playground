using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utility;
using Utility.Messages;

namespace Master
{
    public partial class PartitaMaster : Partita
    {

        public PartitaMaster() : base()
        {
            InitializeComponent();
        }

        #region BindComponent - Draw


        protected void Draw(ChatCoordinamentoMaster chatCoordinamento)
        {
            chatCoordinamento.Output += MessageForwarder;
            _components.Add(chatCoordinamento);
            chatCoordinamento.Height = chatCoordinamento.DisplayHeight;
            chatCoordinamento.Anchor = AnchorStyles.Top;
            _flowPanel.Controls.Add(chatCoordinamento);
        }

        protected void Draw(GestoreSchedeMaster gestoreSchede)
        {
            gestoreSchede.Output += MessageForwarder;
            _components.Add(gestoreSchede);
            gestoreSchede.Anchor = AnchorStyles.Top;
            _flowPanel.Controls.Add(gestoreSchede);
            gestoreSchede.Height = gestoreSchede.DisplayHeight;
        }

        protected void Draw(MyServerSocket myServerSocket)
        {
            myServerSocket.Output += SocketForwarder;
            _components.Add(myServerSocket);
        }

        #endregion

        #region SocketForwarder
        protected override void SocketMessageHandler(MySocket sender, ChatComuneMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is ChatComune || c is MySocket) c.Input(e); //   
        }

        protected virtual void SocketMessageHandler(MyServerSocket sender, Utility.Messages.ServerSocketClosedMessageEventArgs e)
        {
            DocumentoMaster.GetIstance().LeavePartita(this, e);
        }

        protected virtual void SocketMessageHandler(MyServerSocket sender, Utility.Messages.ServerSocketErrorMessageEventArgs e)
        {
            DocumentoMaster.GetIstance().LeavePartita(this, e);
        }

        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.SocketClosedMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components)
                if (c is GestoreSchedeMaster) c.Input(new GestoreGiocatoriMessageEventArgs(e.Argomento, e));
            sender.Dispose();
            _components.Remove(sender);
        }

        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.SocketErrorMessageEventArgs e)
        {
            String nome="";
            foreach (Utility.IComponent c in _components)
                if (c is GestoreSchedeMaster)
                {
                    nome = (c as GestoreSchedeMaster).getNomeGiocatore(e.Argomento);
                    c.Input(new GestoreGiocatoriMessageEventArgs(e.Argomento, e));
                }
            sender.Dispose();
            _components.Remove(sender);
            this.MessageForwarder(sender, new ChatComuneMessageEventArgs(nome + " ha abbandonato la partita"));
        }

        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.PasswordSubmitMessageEventArgs e)
        {
            if (DocumentoMaster.GetIstance().CurrentDescrittore.IdPartita == e.IdPartita)
                if (DocumentoMaster.GetIstance().CurrentDescrittore.Password == e.Argomento) sender.Input(new Utility.Messages.PasswordResponseMessageEventArgs("ok"));
                else sender.Input(new Utility.Messages.PasswordResponseMessageEventArgs("wrong password"));
            else sender.Input(new Utility.Messages.PasswordResponseMessageEventArgs("wrong id"));
        }

        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.GiocatoreCollegatoMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components)
                if (c is GestoreSchedeMaster) c.Input(new GestoreGiocatoriMessageEventArgs(sender.RemoteAddress, e));
            this.MessageForwarder(sender,new ChatComuneMessageEventArgs(e.NomeGiocatore+" partecipa ora alla partita \""+DocumentoMaster.GetIstance().CurrentDescrittore.Nome+"\""));
        }

        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.GiocatoreProntoMessageEventArgs e)
        {
            foreach (Utility.IComponent c in GetComponentsByType(typeof(GestoreSchedeMaster)))
                c.Input(new GestoreGiocatoriMessageEventArgs(sender.RemoteAddress, e));
        }

        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.NomeAggiornatoMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is GestoreSchedeMaster) c.Input(new GestoreGiocatoriMessageEventArgs(sender.RemoteAddress, e));
        }

        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.ModificaCampoSchedaMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is GestoreSchedeMaster) c.Input(e);
        }

        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.RichiestaModificaCampoSchedaMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is GestoreSchedeMaster) c.Input(e);
        }

        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.RichiestaNuovaSchedaMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is GestoreSchedeMaster) 
                c.Input(new Utility.Messages.GestoreGiocatoriMessageEventArgs(sender.RemoteAddress,e));
        }
        #endregion

        #region MessageForwarder

        protected override void MessageHandler(Utility.IComponent sender, ChatCoordinamentoMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is MySocket) c.Input(e);
        }

        protected override void MessageHandler(Utility.IComponent sender, ChatComuneMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is MySocket || c is ChatComune) c.Input(e);
        }

        protected virtual void MessageHandler(Utility.IComponent sender, GestoreGiocatoriMessageEventArgs e)
        {
            foreach (Utility.IComponent c in GetComponentsByType(typeof(MySocket)))
                if ((c as MySocket).RemoteAddress == e.Argomento)
                {
                    c.Input(e.Messaggio);
                }
        }

        protected virtual void MessageHandler(Utility.IComponent sender, GiocatoreKickedMessageEventArgs e)
        {
            Utility.MySocket toRemove = null;
            foreach (Utility.IComponent c in _components)
                if (c is MySocket && (c as MySocket).RemoteAddress == e.Argomento)
                {
                    toRemove = (c as MySocket);
                    if (sender is GestoreSchedeMaster)
                    {
                        this.MessageForwarder(sender, new ChatComuneMessageEventArgs("Il giocatore " + (sender as GestoreSchedeMaster).getNomeGiocatore(e.Argomento) + " è stato espulso"));
                    }
                }
            toRemove.Dispose();
            _components.Remove(toRemove);
        }

        protected virtual void MessageHandler(Utility.IComponent sender, Utility.Messages.NomeAggiornatoMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is MySocket) c.Input(e);
        }

        protected virtual void MessageHandler(Utility.IComponent sender, Utility.Messages.ModificaCampoSchedaMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is GestoreSchedeMaster) c.Input(e);
        }
        #endregion


        protected override void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        //dispose managed resources
                        foreach (Utility.IComponent c in _components)
                        {
                            if (c != null && c is IDisposable)
                            {
                                if (c is MySocket) (c as MySocket).NotifyDispose = false;
                                if (c is MyServerSocket) (c as MyServerSocket).NotifyDispose = false;
                                (c as IDisposable).Dispose();
                            }
                        }
                        if (_flowPanel != null) _flowPanel.Dispose();
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
