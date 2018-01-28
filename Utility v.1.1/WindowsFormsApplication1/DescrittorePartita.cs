using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public class DescrittorePartita
    {
        private String _idPartita;
        private String _nome;
        private String _nomeMaster;
        private String _ipMaster;
        private String _descrizione;
        private String _password;
        private StatoPartita _stato;

        #region Properties
        public String IdPartita
        {
            get { return _idPartita; }
            set { _idPartita = value; }
        }
        

        public String Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }
        

        public String NomeMaster
        {
            get { return _nomeMaster; }
            set { _nomeMaster = value; }
        }
        

        public String IpMaster
        {
            get { return _ipMaster; }
            set { _ipMaster = value; }
        }
        

        public String Descrizione
        {
            get { return _descrizione; }
            set { _descrizione = value; }
        }
        

        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }
        

        public StatoPartita Stato
        {
            get { return _stato; }
            set { _stato = value; }
        }
        #endregion

        public override string ToString()
        {
            return "[" + _idPartita + ", " + _nome + ", " + _nomeMaster + ", " + _ipMaster + ", " + _descrizione + ", " + _password + ", " + _stato + "]";
        }
    }

    public enum StatoPartita { Attiva, Inattiva, Terminata }
    
}
