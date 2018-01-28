using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using System.Windows.Forms;

namespace Giocatore
{
    public class PartitaGiocatore : Partita
    {
        public PartitaGiocatore() : base() {
        }

        #region BindComponent - Draw
        protected void Draw(ChatCoordinamentoGiocatore chatCoordinamento)
        {
            _components.Add(chatCoordinamento);
            chatCoordinamento.Height = chatCoordinamento.DisplayHeight;
            chatCoordinamento.Anchor = AnchorStyles.Top;
            _flowPanel.Controls.Add(chatCoordinamento);
        }
        #endregion

        #region SocketForwarder
        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.ChatCoordinamentoMessageEventArgs e)
        {
            foreach (IComponent c in _components) if (c is ChatCoordinamentoGiocatore) c.Input(e);
        }

        protected override void SocketMessageHandler(MySocket sender, Utility.Messages.SocketClosedMessageEventArgs e)
        {
            sender.Dispose();
            _components.Remove(sender);
            foreach (IComponent c in _components) if (c is ChatComune) c.Input(new Utility.Messages.ChatComuneMessageEventArgs("Il Master ha chiuso la connessione"));
        }

        protected override void SocketMessageHandler(MySocket sender, Utility.Messages.SocketErrorMessageEventArgs e)
        {
            sender.Dispose();
            _components.Remove(sender);
            foreach (IComponent c in _components) if (c is ChatComune) c.Input(new Utility.Messages.ChatComuneMessageEventArgs("Il Master ha chiuso la connessione"));
        }

        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.NomeAggiornatoMessageEventArgs e)
        {
            this.MessageForwarder(sender,new Utility.Messages.ChatComuneMessageEventArgs("Il Master ha cambiato il suo nome in: "+e.Argomento));
        }
        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.ModificaCampoSchedaMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is GestoreSchedeGiocatore) c.Input(e);
        }
        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.CambiaNomeSchedaMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is GestoreSchedeGiocatore) c.Input(e);
        }
        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.NuovaSchedaRifiutataMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is GestoreSchedeGiocatore) c.Input(e);
        }
        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.NuovaSchedaConfermataMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is GestoreSchedeGiocatore) c.Input(e);
        }
        protected virtual void SocketMessageHandler(MySocket sender, Utility.Messages.SchedaCancellataMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is GestoreSchedeGiocatore) c.Input(e);
        }
        #endregion

        #region MessageForwarder
        protected virtual void MessageHandler(IComponent sender, Utility.Messages.NomeAggiornatoMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is MySocket) c.Input(e);
        }

        protected virtual void MessageHandler(IComponent sender, Utility.Messages.ModificaCampoSchedaMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is MySocket || c is GestoreSchedeGiocatore) c.Input(e);
        }
        protected virtual void MessageHandler(IComponent sender, Utility.Messages.RichiestaModificaCampoSchedaMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is MySocket) c.Input(e);
        }
        protected virtual void MessageHandler(IComponent sender, Utility.Messages.RichiestaNuovaSchedaMessageEventArgs e)
        {
            foreach (Utility.IComponent c in _components) if (c is MySocket) c.Input(e);
        }
        #endregion

    }
}
