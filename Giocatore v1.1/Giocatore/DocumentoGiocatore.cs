using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using System.Windows.Forms;
using System.IO;

namespace Giocatore
{
    public class DocumentoGiocatore : Documento
    {
        private MySocket _socket;
        private PartitaGiocatore _currentPartita;

        private DocumentoGiocatore():base(){
            _currentPartita = null;
            _mainForm = new MainFormGiocatore();
            try
            {
                _mainForm.Descrittori = _persister.LoadDescrittorePartita(_path + "Descrittori.xml");
            }
            catch { _persister.SaveDescrittorePartita(_path + "Descrittori.xml", new List<DescrittorePartita>()); }
            System.Drawing.Rectangle resolution = Screen.PrimaryScreen.Bounds;
            _mainForm.Location = new System.Drawing.Point(resolution.Width - 800, 0);

        }
        public static new DocumentoGiocatore GetIstance() {
            if (_doc == null) _doc = new DocumentoGiocatore();
            return _doc as DocumentoGiocatore;
        }

        #region properties
        public PartitaGiocatore CurrentPartita
        {
            get { return _currentPartita; }
            set { _currentPartita = value; }
        }
        public new string NomeUtente
        {
            get { return _nomeUtente; }
            set 
            {
                _nomeUtente = value;
                if (_currentPartita != null && _socket != null) _currentPartita.MessageForwarder(_socket, new Utility.Messages.NomeAggiornatoMessageEventArgs(value));
            }
        }
        #endregion

        public override void JoinPartita(DescrittorePartita descrittore)
        {
            base.JoinPartita(descrittore);
            try
            {
                _socket = new MySocket(descrittore.IpMaster, PortEnum.Master);
            }
            catch (System.Net.Sockets.SocketException)
            {
                MessageBox.Show("Impossibile connettersi al Master");
                LeavePartita(this,new EventArgs());
                return;
            }
            if (!_socket.isConnected())
            {
                return;
            }
            _socket.Output+=MasterHandShake;
            _socket.Input(new Utility.Messages.PasswordSubmitMessageEventArgs(descrittore.Password,descrittore.IdPartita));

        }

        private string newPassword = null;
        private String templateRecived = null;
        private String schedeRecived = null;
        private void MasterHandShake(Object o, Utility.Messages.MessageEventArgs message) 
        {
            try
            {
                if ((message is Utility.Messages.PasswordResponseMessageEventArgs))
                {
                    Utility.Messages.PasswordResponseMessageEventArgs passResponse = message as Utility.Messages.PasswordResponseMessageEventArgs;
                    if (passResponse.Argomento.ToLower() != "ok")
                    {
                        if (passResponse.Argomento.ToLower().Equals("wrong id"))
                        {
                            LeavePartita(this, new EventArgs());
                        }
                        else
                        {
                            this.newPassword = SpawnInputDialog();
                            if (this.newPassword != null) _socket.Input(new Utility.Messages.PasswordSubmitMessageEventArgs(this.newPassword, CurrentDescrittore.IdPartita));
                            else LeavePartita(this, new EventArgs());
                        }
                    }
                    else
                    {
                        if (CurrentDescrittore == null)
                        {
                            LeavePartita(this, new EventArgs());
                            return;
                        }
                        if (this.newPassword != null)
                        {
                            CurrentDescrittore.Password = this.newPassword;
                            _persister.SaveDescrittorePartita(_path + "Descrittori.xml", _mainForm.Descrittori);
                        }
                        RequestTemplateAndSchede();
                    }
                }
                else if (message is Utility.Messages.PersistanceFileTemplateMessageEventArgs)
                {
                    _persister.SaveTemplate(_path + (CurrentDescrittore.IdPartita) + "/Template.xml", _persister.FormatTemplate(message.Argomento));
                    this.templateRecived = message.Argomento;
                    if (!String.IsNullOrEmpty(this.schedeRecived)) InitPartita();
                }
                else if (message is Utility.Messages.PersistanceFileSchedeMessageEventArgs)
                {
                    _persister.SaveScheda(_path + (CurrentDescrittore.IdPartita) + "/Schede.xml", _persister.FormatScheda(message.Argomento));
                    this.schedeRecived = message.Argomento;
                    if (!String.IsNullOrEmpty(this.templateRecived)) InitPartita();
                }
                else
                {
                    LeavePartita(this,new EventArgs());
                }
            }
            catch(Exception)
            {
                LeavePartita(this, new EventArgs());
            }

        }

        private void RequestTemplateAndSchede()
        {
            IEnumerable<Scheda> schede = null;
            try
            {
                schede = _persister.LoadScheda(_path + CurrentDescrittore.IdPartita + "/Schede.xml");
            }
            catch
            {
                Persister.SaveScheda(_path + CurrentDescrittore.IdPartita + "/Schede.xml", new List<Scheda>());
                schede = _persister.LoadScheda(_path + CurrentDescrittore.IdPartita + "/Schede.xml");
            }
            List<string> ids= new List<string>();
            ids.AddRange(from s in schede select s.IdScheda);
            _socket.Input(new Utility.Messages.GiocatoreCollegatoMessageEventArgs(CurrentDescrittore.IdPartita, _nomeUtente, ids));//invia gli ID al Master

        }

        private string SpawnInputDialog() {
            String value="";
            return Dialog.InputBox("Inserire Password", "Inserire Password", ref value) == System.Windows.Forms.DialogResult.OK ? value : null;
        }

        Form form = null;
        private void InitPartita()
        {
            _socket.Input(new Utility.Messages.GiocatoreProntoMessageEventArgs(_nomeUtente));
            _socket.Output -= MasterHandShake;
            _currentPartita = new PartitaGiocatore();
            _currentPartita.Dock = DockStyle.Fill;
            _currentPartita.BindComponent(_socket);
            GestoreSchedeGiocatore gestoreSchede = new GestoreSchedeGiocatore();
            gestoreSchede.Carica(_path + CurrentDescrittore.IdPartita);
            _currentPartita.BindComponent(gestoreSchede);
            ChatComune chatComune = new ChatComune();
            _currentPartita.BindComponent(chatComune);
            ChatCoordinamentoGiocatore chatCoordinamentoGiocatore = new ChatCoordinamentoGiocatore();
            _currentPartita.BindComponent(chatCoordinamentoGiocatore);
            
            this.form = new Form();
            this.form.FormClosing += gestoreSchede.Salva;
            this.form.FormClosed += LeavePartita;
            this.form.Controls.Add(_currentPartita);
            this.form.Size = new System.Drawing.Size(800, 600);
            System.Drawing.Rectangle resolution = Screen.PrimaryScreen.Bounds;
            this.form.Location = new System.Drawing.Point(resolution.Width - 800, resolution.Height - 650);
            this.form.Text = CurrentDescrittore.Nome;
            this.form.Show();


        }
        public override void LeavePartita(object sender, EventArgs e) 
        {
            this.newPassword = null;
            this.templateRecived = null;
            this.schedeRecived = null;
            CurrentDescrittore = null;
            if (_currentPartita != null)
            {
                if (_currentPartita != null) _currentPartita.Dispose();
                if (this.form != null) this.form.Dispose();
            }
            if (_socket != null ) _socket.Dispose();
            base.LeavePartita(sender,e);
        }
        
    }
}
