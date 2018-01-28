using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Utility
{
    public class Scheda
    {
        private String _idScheda;

        
        private readonly Dictionary<String,String> _valori;

        public Scheda(String idScheda)
        {
            _idScheda = idScheda;
            _valori = new Dictionary<String, String>();
        }

        public String IdScheda
        {
            get { return _idScheda; }
            set { _idScheda = value; }
        }

        public IEnumerable<String> GetIds()
        {
            return _valori.Keys;
        }

        public String GetValore(String id)
        {
            if (String.IsNullOrEmpty(id)) throw new ArgumentException("id is Null or Empty");
            return _valori[id];
        }

        public void SetValore(String id, String valore)
        {
            if (String.IsNullOrEmpty(id)) throw new ArgumentException("id is Null or Empty");
            _valori[id] = valore;
        }
    }
}
