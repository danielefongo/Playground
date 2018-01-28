using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Utility
{
    public class GestoreGiocatori
    {
        private Dictionary<String, Giocatore> _giocatori;

        public GestoreGiocatori()
        {
            _giocatori = new Dictionary<String, Giocatore>();
        }

        public void AddGiocatore(String ip)
        {
            if (!_giocatori.ContainsKey(ip)) _giocatori.Add(ip, new Giocatore(ip));
            else _giocatori[ip] = new Giocatore(ip);
        }

        public void AddGiocatore(String ip, IEnumerable<Scheda> schede)
        {
            AddGiocatore(ip);
            _giocatori[ip].AddSchede(schede.ToArray());
        }

        public void RemoveGiocatore(String ip)
        {
            if (_giocatori.ContainsKey(ip)) _giocatori.Remove(ip);
        }

        public IEnumerable<String> GetGiocatori()
        {
            return _giocatori.Keys;
        }

        public String GetNomeGiocatore(String ip)
        {
            if (_giocatori.ContainsKey(ip))
            {
                if (String.IsNullOrEmpty(_giocatori[ip].Nome)) return "Giocatore " + (_giocatori.Keys.ToList<String>().IndexOf(ip)+1);
                else return _giocatori[ip].Nome;
            }
            return null;
        }

        public void SetNomeGiocatore(String ip, String nome)
        {
            if (_giocatori.ContainsKey(ip)) _giocatori[ip].Nome = nome;
        }

        #region Connections
        public void SetConnected(String ip)
        {
            if (_giocatori.ContainsKey(ip)) _giocatori[ip].Connected = true;
        }

        public void SetDisconnected(String ip)
        {
            if (_giocatori.ContainsKey(ip)) _giocatori[ip].Connected = false;
        }

        public void ToggleConnected(String ip)
        {
            if (_giocatori.ContainsKey(ip)) _giocatori[ip].Connected = !_giocatori[ip].Connected;
        }

        public Boolean IsConnected(String ip)
        {
            if (_giocatori.ContainsKey(ip)) return _giocatori[ip].Connected;
            else return false;
        }
        #endregion

        public void AddSchede(String ip, params Scheda[] schede)
        {
            if (_giocatori.ContainsKey(ip)) _giocatori[ip].AddSchede(schede);
        }

        public void RemoveSchede(String ip, params Scheda[] schede)
        {
            if (_giocatori.ContainsKey(ip)) _giocatori[ip].RemoveSchede(schede);
        }

        public IEnumerable<Scheda> GetSchede(String ip)
        {
            if (_giocatori.ContainsKey(ip)) return _giocatori[ip].GetSchede();
            return new List<Scheda>();
        }

        public String getIpGiocatoreFromScheda(String idScheda)
        {
            foreach(Giocatore giocatore in _giocatori.Values)
                foreach(Scheda scheda in giocatore.GetSchede())
                    if(scheda.IdScheda==idScheda)return giocatore.Ip;
            return null;
        }

        #region CLASSE-GIOCATORE
        public class Giocatore
        {
            private readonly String _ip;
            private String _nome;
            private List<Scheda> _schede;
            private Boolean _connected;

            #region Properties

            public Boolean Connected
            {
                get { return _connected; }
                set { _connected = value; }
            }

            public String Ip
            {
                get { return _ip; }
            }

            public String Nome
            {
                get { return _nome; }
                set { _nome = value; }
            }
            #endregion

            public Giocatore(String ip)
            {
                _schede = new List<Scheda>();
                _ip = ip;
            }

            public void AddSchede(params Scheda[] schede)
            {
                _schede.AddRange(from s in schede where !_schede.Contains(s) select s);
            }

            public void RemoveSchede(params Scheda[] schede)
            {
                foreach (Scheda s in schede) if (_schede.Contains(s)) _schede.Remove(s);
            }

            public IEnumerable<Scheda> GetSchede()
            {
                return _schede;
            }

        }
        #endregion
    }
    
    
}

namespace Utility.Messages
{
    [Serializable]
    public class GiocatoreKickedMessageEventArgs : MessageEventArgs
    {
        public GiocatoreKickedMessageEventArgs(String argomento) : base(argomento) { }
        public GiocatoreKickedMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class GestoreGiocatoriMessageEventArgs : MessageEventArgs
    {
        private readonly MessageEventArgs _messaggio;

        public MessageEventArgs Messaggio { get { return _messaggio; } }

        public override String ToString()
        {
            String temp = this.Argomento + ":" + _messaggio.ToString() + ":";
            return temp;
        }

        public GestoreGiocatoriMessageEventArgs(String argomento, MessageEventArgs messaggio) : base(argomento) { _messaggio = messaggio; }
        public GestoreGiocatoriMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    
}