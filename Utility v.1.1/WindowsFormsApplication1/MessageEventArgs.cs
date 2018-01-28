using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Utility.Messages;

namespace Utility
{
    public delegate void Evento(Object sender, MessageEventArgs argomenti);
}

namespace Utility.Messages
{
    
    [Serializable]
    public class MessageEventArgs : EventArgs, ISerializable
    {

        protected readonly String _argomento;

        public MessageEventArgs(String argomento)
        {
            try
            {
                if (argomento == null) throw new ArgumentNullException("MyEventArgs: argomento nullo");
                else _argomento = argomento;
            }
            catch (Exception) { ; }
            
        }
        
        public MessageEventArgs(SerializationInfo info, StreamingContext context)
        {
            _argomento=(String)info.GetValue("message", typeof(String));
        }

        #region Properties
        public String Argomento
        {
            get { return _argomento; }
        }
        #endregion

        public override String ToString() { return _argomento; }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("message", _argomento, typeof(String));
        }
    }


    [Serializable]
    public class ChatCoordinamentoMessageEventArgs : MessageEventArgs
    {
        public ChatCoordinamentoMessageEventArgs(String argomento) : base(argomento) { }
        public ChatCoordinamentoMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class PasswordSubmitMessageEventArgs : MessageEventArgs, ISerializable
    {
        private readonly String _idPartita;

        public String IdPartita { get { return _idPartita; } }

        public override String ToString()
        {
            String temp = this.Argomento + ":" + this.IdPartita + ":";
            return temp;
        }

        public PasswordSubmitMessageEventArgs(String argomento, String idPartita) : base(argomento) { _idPartita = idPartita; }
        public PasswordSubmitMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { _idPartita = (String)info.GetValue("id", typeof(String)); }
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("message", _argomento, typeof(String));
            info.AddValue("id", _idPartita, typeof(String));
        }
    }
    
    [Serializable]
    public class PasswordResponseMessageEventArgs : MessageEventArgs
    {
        public PasswordResponseMessageEventArgs(String argomento) : base(argomento) { }
        public PasswordResponseMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class GiocatoreCollegatoMessageEventArgs : MessageEventArgs, ISerializable
    {
        private readonly List<String> _schede;
        private readonly String _nomeGiocatore;

        public List<String> Schede { get { return _schede; } }
        public String NomeGiocatore { get { return _nomeGiocatore; } } 

        public override String ToString()
        {
            String temp = this.Argomento + ":" + this.NomeGiocatore + ":";
            foreach(String s in _schede)
            {
                temp = temp + s + "§";
            }
            return temp;
        }

        public GiocatoreCollegatoMessageEventArgs(String argomento, String nomeGiocatore, List<String> schede) : base(argomento) { _nomeGiocatore = nomeGiocatore; _schede = schede; }
        public GiocatoreCollegatoMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { 
            _nomeGiocatore = (String)info.GetValue("nomeGiocatore", typeof(String));
            _schede = (List<String>)info.GetValue("schede", typeof(List<String>));
        }
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("message", _argomento, typeof(String));
            info.AddValue("nomeGiocatore", _nomeGiocatore, typeof(String));
            info.AddValue("schede", _schede, typeof(List<String>));
        }
    }

    [Serializable]
    public class GiocatoreProntoMessageEventArgs : MessageEventArgs
    {
        public GiocatoreProntoMessageEventArgs(String argomento) : base(argomento) { }
        public GiocatoreProntoMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class NomeAggiornatoMessageEventArgs : MessageEventArgs
    {
        public NomeAggiornatoMessageEventArgs(String argomento) : base(argomento) { }
        public NomeAggiornatoMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
