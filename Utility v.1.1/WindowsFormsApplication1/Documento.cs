using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;
using System.IO;

namespace Utility
{
    public class Documento
    {
        protected static Documento _doc;
        protected readonly IPersister _persister;
        protected string _path;
        protected string _nomeUtente;
        protected MainForm _mainForm;
        
        private DescrittorePartita _currentDescrittore = null;
        private  SynchronizationContext _syncContext;

        #region Properties

        public string NomeUtente
        {
            get { return _nomeUtente; }
            set { _nomeUtente = value; }
        }
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        public MainForm MainForm
        {
            get { return _mainForm; }
            set { _mainForm = value; }
        }
       
        public IPersister Persister
        {
            get { return _persister; }
        }

        public DescrittorePartita CurrentDescrittore
        {
            get { return _currentDescrittore; }
            set { _currentDescrittore = value; }
        }
        public SynchronizationContext SyncContext
        {
            get { return _syncContext != null ? _syncContext : _syncContext = AsyncOperationManager.SynchronizationContext; }
        }

        #endregion

        public static Documento GetIstance()
        {
            if (_doc == null) _doc = new Documento();
            return _doc;
        }


        
        public Documento()
        {
            _persister = new XmlPersister();
            _nomeUtente = "Default";
            _path = "";
        }

        public virtual void JoinPartita(DescrittorePartita descrittore)
        {
            _mainForm.AllowJoin=false;
            _currentDescrittore = descrittore;
        }

        public virtual void LeavePartita(object sender, EventArgs e)
        {
            _currentDescrittore = null;
            _mainForm.AllowJoin=true;
            
        }
    }

}
