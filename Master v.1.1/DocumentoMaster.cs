using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;

namespace Master
{
    public class DocumentoMaster : Documento
    {
        private PartitaMaster _currentPartita;
        private ChatComune _chatComune;
        private ChatCoordinamentoMaster _chatCoordinamentoMaster;
        private GestoreSchedeMaster _gestoreSchede;
        private MyServerSocket _serverSocket;
        private Form _partitaForm;

        #region Properties
        public PartitaMaster CurrentPartita
        {
            get { return _currentPartita; }
        }
        public Form PartitaForm
        {
            get { return _partitaForm; }
            set { _partitaForm = value; }
        }
        public new string NomeUtente
        {
            get { return _nomeUtente; }
            set
            {
                _nomeUtente = value;
                if (_currentPartita != null && _serverSocket != null) _currentPartita.MessageForwarder(_serverSocket, new Utility.Messages.NomeAggiornatoMessageEventArgs(value));
            }
        }
        #endregion

        public DocumentoMaster() : base()
        {
            _mainForm = new MainFormMaster();
            try
            {
                _mainForm.Descrittori = _persister.LoadDescrittorePartita(_path + "Descrittori.xml");
            }
            catch 
            {
                _persister.SaveDescrittorePartita(_path + "Descrittori.xml", new List<DescrittorePartita>()); 
            }
            _mainForm.Location = new System.Drawing.Point(0, 0);

            
        }

        public static new DocumentoMaster GetIstance()
        {
            if (_doc == null) _doc = new DocumentoMaster();
            return _doc as DocumentoMaster;
        }


        public override void JoinPartita(DescrittorePartita descrittore)
        {
            base.JoinPartita(descrittore);
            if(_currentPartita == null)
            try
            {
                _chatComune = new ChatComune();
                _chatCoordinamentoMaster = new ChatCoordinamentoMaster();
                _gestoreSchede = new GestoreSchedeMaster();
                _gestoreSchede.Dock = DockStyle.Fill;
                _gestoreSchede.Carica(_path + descrittore.IdPartita + "/");

                _currentPartita = new PartitaMaster();
                _currentPartita.Dock = DockStyle.Fill;
                _currentPartita.BindComponent(_gestoreSchede);
                _currentPartita.BindComponent(_chatComune);
                _currentPartita.BindComponent(_chatCoordinamentoMaster);
                                
                _serverSocket = new MyServerSocket("127.0.0.1", PortEnum.Master);
                _currentPartita.BindComponent(_serverSocket);
                
                _partitaForm = new Form();
                _partitaForm.Controls.Add(_currentPartita);
                _partitaForm.Size = new System.Drawing.Size(800, 600);
                System.Drawing.Rectangle resolution = Screen.PrimaryScreen.Bounds;
                _partitaForm.Location = new System.Drawing.Point(0, resolution.Height - 650);
                _partitaForm.Text = descrittore.Nome;
                _partitaForm.FormClosing += _gestoreSchede.Salva;
                _partitaForm.FormClosed += LeavePartita;
                _partitaForm.Show();               
            }
            catch (SocketException)
            {
                MessageBox.Show("Master non trovato", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Errore", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public override void LeavePartita(object sender, EventArgs e)
        {
            base.LeavePartita(sender, e);
            if (_currentPartita != null) _currentPartita.Dispose();
            if(_partitaForm!=null)_partitaForm.Dispose();
          
            _currentPartita = null;
            _serverSocket = null;
            _partitaForm = null;
        }

        public void CloseApplication()
        {
            if(_partitaForm!=null) _partitaForm.Close();
            Application.Exit();
        }
    }
}
