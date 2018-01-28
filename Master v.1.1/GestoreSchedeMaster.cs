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
    public partial class GestoreSchedeMaster : GestoreSchede
    {
        private GestoreGiocatori _gestoreGiocatori;
        private Button _kickButton, _cancellaSchedaButton;
        private Dictionary<String, String> _schedePendenti; //nome-ip
        

        public GestoreSchedeMaster() : base()
        {
            InitializeComponent();
            _gestoreGiocatori = new GestoreGiocatori();
            _schedePendenti = new Dictionary<String, String>();
        }
        protected override void init()
        {
            base.init();
            Button saveButton = new Button();
            saveButton.Text = "Salva schede";
            saveButton.Dock = DockStyle.Bottom;
            saveButton.Click += Salva;
            _splitter.Panel1.Controls.Add(saveButton);

            _cancellaSchedaButton = new Button();
            _cancellaSchedaButton.Text = "Cancella Scheda";
            _cancellaSchedaButton.Dock = DockStyle.Top;
            _cancellaSchedaButton.Click += CancellaScheda;
            
        }

        protected override void DisplayContenuto(Scheda scheda, TableLayoutPanel table, Contenuto con, int level)
        {
            String tabs = "";
            for (int i = 0; i < level; i++) tabs += "      ";

            String[] currentId=new String[2]{scheda.IdScheda,con.Id};

            String key = con.Id;
            Label l = new Label();
            l.AutoSize = true;
            l.Text = tabs + key;
            
            table.Controls.Add(l);

            Label l2 = new Label();
            l2.AutoSize = true;
            l2.Text = _values[GenerateId(currentId)][0]+"";
            table.Controls.Add(l2);

            TextBox tb = new TextBox();
            tb.Text = _values[GenerateId(currentId)][1] == null ? "" : _values[GenerateId(currentId)][1];
            
            table.Controls.Add(tb);

            String[] ProprietaTag = new String[] { scheda.IdScheda, con.Id, scheda.GetValore(con.Id), null };
            //ID scheda | ID campo | Valore vecchio | Valore nuovo

            Button conferma = new Button();
            conferma.Text = "Conferma";
            conferma.AutoSize = true;
            conferma.Tag = new Object[] { currentId, tb };
            conferma.Click += ModificaCampoScheda;
            table.Controls.Add(conferma);

            Button cancella = new Button();
            cancella.Text = "Cancella";
            cancella.AutoSize = true;
            cancella.Tag = new Object[] {currentId,tb};
            cancella.Click += RipristinaCampoScheda;
            table.Controls.Add(cancella);

            if (_values[GenerateId(currentId)][1] != null) tb.BackColor = Color.Yellow;
        }
        protected override void ModificaCampoScheda(object sender, EventArgs e)
        {
            String[] Id = ((sender as Control).Tag as Object[])[0] as String[];
            String newValue = (((sender as Control).Tag as Object[])[1] as TextBox).Text;
            if (String.IsNullOrEmpty(newValue)) return;
            
            _output(this, new GestoreGiocatoriMessageEventArgs(_gestoreGiocatori.getIpGiocatoreFromScheda(Id[0]),
                new Utility.Messages.ModificaCampoSchedaMessageEventArgs(Id[0],Id[1], newValue)));
            _output(this,new Utility.Messages.ModificaCampoSchedaMessageEventArgs(Id[0], Id[1], newValue));
        }
        protected override void RipristinaCampoScheda(object sender, EventArgs e)
        {
            String[] Id = ((sender as Control).Tag as Object[])[0] as String[];
            String newValue = _values[GenerateId(Id)][0];
            _output(this, new GestoreGiocatoriMessageEventArgs(_gestoreGiocatori.getIpGiocatoreFromScheda(Id[0]),
                new Utility.Messages.ModificaCampoSchedaMessageEventArgs(Id[0], Id[1], newValue)));
            _output(this, new Utility.Messages.ModificaCampoSchedaMessageEventArgs(Id[0], Id[1], newValue));
        }
        protected virtual void CancellaScheda(object sender, EventArgs e)
        {
            if(_schedeView.SelectedNode!=null && _schedeView.SelectedNode.Tag is Scheda)
                if (MessageBox.Show("Sei sicuro di voler eliminare questa scheda?", "Eliminazione scheda", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Scheda condannata = (_schedeView.SelectedNode.Tag as Scheda);
                    String ip = (_schedeView.SelectedNode.Parent.Tag as String);
                    _output(this, new GestoreGiocatoriMessageEventArgs(ip, new SchedaCancellataMessageEventArgs(condannata.IdScheda)));
                    RemoveScheda(condannata);
                    InitValues();
                    if(ip!=null)_gestoreGiocatori.RemoveSchede(ip, condannata);
                    _schedaPanel.Controls.Clear();
                    RefreshSchedeView(_schedeView);
                    SalvaSchede();
                }
            
        }

        protected override void OnElementSelected(object sender, EventArgs e)
        {
            if (sender is TreeView && ((TreeView)sender).SelectedNode.Tag is String)
            {
                if(_kickButton == null) 
                {
                    _kickButton = new Button();
                    _kickButton.AutoSize = true;
                    _kickButton.Anchor = AnchorStyles.Top;
                    _kickButton.Click += KickGiocatore;
                }
                _kickButton.Text = "Espelli " + _schedeView.SelectedNode.Text;
                _schedaPanel.Controls.Add(_kickButton);
            }
            if (sender is TreeView && ((TreeView)sender).SelectedNode.Tag == null && ((TreeView)sender).SelectedNode.Parent != null)
            {
                Button conferma = new Button();
                Button cancella = new Button();
                cancella.Location = new Point(0, conferma.Height+10);
                conferma.AutoSize = true;
                cancella.AutoSize = true;
                conferma.Click += ConfermaNuovaScheda;
                cancella.Click += CancellaNuovaScheda;
                conferma.Text = "Conferma Nuova Scheda";
                cancella.Text = "Cancella Nuova Scheda";
                conferma.Tag = ((TreeView)sender).SelectedNode.Parent.Tag as String;
                cancella.Tag = ((TreeView)sender).SelectedNode.Parent.Tag as String;
                _schedaPanel.Controls.Add(conferma);
                _schedaPanel.Controls.Add(cancella);
            }
        }

        protected override void RefreshSchedeView(TreeView tree) 
        {
            tree.Nodes.Clear();
            foreach (String ip in _gestoreGiocatori.GetGiocatori())
            {
                TreeNode parentnode = new TreeNode();
                parentnode.Tag = ip;
                if (!_gestoreGiocatori.IsConnected(ip)) parentnode.Text = _gestoreGiocatori.GetNomeGiocatore(ip) + " - Connessione in corso";
                else
                {
                    parentnode.Text = _gestoreGiocatori.GetNomeGiocatore(ip);
                    foreach (Scheda s in _gestoreGiocatori.GetSchede(ip))
                    {
                        TreeNode node = new TreeNode();
                        node.Tag = s;
                        node.Text = s.IdScheda;
                        parentnode.Nodes.Add(node);
                        foreach(String id in s.GetIds())
                            if (_values[GenerateId(s.IdScheda, id)][1] != null)
                            {
                                node.BackColor = Color.Yellow;
                                parentnode.BackColor = Color.Yellow;
                                break;
                            }
                    }
                }
                tree.Nodes.Add(parentnode);
            }
            AddSchedePendenti(tree);
            ColoraSchedeView(tree.Nodes);
            tree.ExpandAll();
        }

        protected override void AddSchedePendenti(TreeView tree)
        {
            foreach (String nome in _schedePendenti.Keys)
            {
                TreeNode parentnode = null;
                foreach (TreeNode t in tree.Nodes)
                {
                    if (t.Tag == _schedePendenti[nome]) parentnode = t;
                }
                if (parentnode != null)
                {
                    Scheda s = null;
                    TreeNode node = new TreeNode();
                    node.Tag = s;
                    node.Text = nome;
                    node.BackColor = Color.Red;
                    parentnode.Nodes.Add(node);
                }
            }
        }

        protected override void DisplayScheda()
        {
            base.DisplayScheda();
            _schedaPanel.Controls.Add(_cancellaSchedaButton);
        }

        public String getNomeGiocatore(String ip)
        {
            return _gestoreGiocatori.GetNomeGiocatore(ip);
        }

        private void AddGiocatore(String ip, String nome, params String[] idSchede)
        {
            _gestoreGiocatori.AddGiocatore(ip, from Scheda s in _schede.Keys where idSchede.Contains(s.IdScheda) select s);
            _gestoreGiocatori.SetNomeGiocatore(ip, nome);
            RefreshSchedeView(_schedeView);
        }

        private void RemoveGiocatore(String ip)
        {
            _gestoreGiocatori.RemoveGiocatore(ip);
            RefreshSchedeView(_schedeView);
            _schedaPanel.Controls.Clear();
        }

        private void SetGiocatoreConnesso(String ip)
        {
            _gestoreGiocatori.SetConnected(ip);
           RefreshSchedeView(_schedeView);
        }

        private void KickGiocatore(object sender, EventArgs e)
        {
            List<String> schedeDaRimuovere = new List<String>();
            if(_schedeView.SelectedNode!=null)
                foreach (String scheda in _schedePendenti.Keys) if (_schedePendenti[scheda] == (_schedeView.SelectedNode.Tag as String)) schedeDaRimuovere.Add(scheda);
            foreach (String scheda in schedeDaRimuovere) _schedePendenti.Remove(scheda);
            _output(this, new Utility.Messages.GiocatoreKickedMessageEventArgs(_schedeView.SelectedNode.Tag as String));
            RemoveGiocatore(_schedeView.SelectedNode.Tag as String);
            _schedaPanel.Controls.Clear();
        }

        private void ConfermaNuovaScheda(object sender, EventArgs e)
        { 
            if(sender is Button)
            {   
                Button button=sender as Button;
                String ip = (button.Tag as String);
                String name = _schedeView.SelectedNode.Text;

                ElementoTemplate template = _schede.Values.Count > 0 ? _schede.Values.First<ElementoTemplate>() :
                    Documento.GetIstance().Persister.LoadTemplate(Documento.GetIstance().Path + Documento.GetIstance().CurrentDescrittore.IdPartita + "/Template.xml");
                
                Scheda nuovaScheda=GeneraScheda(template, name);
                _schede.Add(nuovaScheda, template);
                _gestoreGiocatori.AddSchede(ip, nuovaScheda);
                _schedePendenti.Remove(name);
                _schedaPanel.Controls.Clear();
                InitValues();
                RefreshSchedeView(_schedeView);

                _output(this,new Utility.Messages.GestoreGiocatoriMessageEventArgs(ip,new NuovaSchedaConfermataMessageEventArgs(name)));

                SalvaSchede();
            }
        }

        private void CancellaNuovaScheda(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button button = sender as Button;
                String ip = (button.Tag as String);
                String name = _schedeView.SelectedNode.Text;

                _schedePendenti.Remove(name);
                _schedaPanel.Controls.Clear();
                RefreshSchedeView(_schedeView);

                _output(this, new Utility.Messages.GestoreGiocatoriMessageEventArgs(ip, new NuovaSchedaRifiutataMessageEventArgs(name)));
            }
        }

        

        protected override void SwitchMessaggio(MessageEventArgs messaggio)
        {
            if (messaggio is GestoreGiocatoriMessageEventArgs)
            {
                if ((messaggio as GestoreGiocatoriMessageEventArgs).Messaggio is GiocatoreCollegatoMessageEventArgs)
                {
                    AddGiocatore(messaggio.Argomento, ((messaggio as GestoreGiocatoriMessageEventArgs).Messaggio as GiocatoreCollegatoMessageEventArgs).NomeGiocatore, ((messaggio as GestoreGiocatoriMessageEventArgs).Messaggio as GiocatoreCollegatoMessageEventArgs).Schede.ToArray());
                    try
                    {
                        InviaTemplateESchede(messaggio.Argomento, ((messaggio as GestoreGiocatoriMessageEventArgs).Messaggio as GiocatoreCollegatoMessageEventArgs).Schede.ToArray());
                    }
                    catch (Exception)
                    {
                        _output(this, new GestoreGiocatoriMessageEventArgs(messaggio.Argomento, new GiocatoreKickedMessageEventArgs("")));
                    }
                }
                if ((messaggio as GestoreGiocatoriMessageEventArgs).Messaggio is GiocatoreProntoMessageEventArgs)
                {
                    SetGiocatoreConnesso(messaggio.Argomento);
                }
                if ((messaggio as GestoreGiocatoriMessageEventArgs).Messaggio is SocketErrorMessageEventArgs || (messaggio as GestoreGiocatoriMessageEventArgs).Messaggio is SocketClosedMessageEventArgs)
                {
                    RemoveGiocatore(messaggio.Argomento);
                }
                if ((messaggio as GestoreGiocatoriMessageEventArgs).Messaggio is NomeAggiornatoMessageEventArgs)
                {
                    String prevName = _gestoreGiocatori.GetNomeGiocatore(messaggio.Argomento);
                    String newName = (messaggio as GestoreGiocatoriMessageEventArgs).Messaggio.Argomento;
                    _gestoreGiocatori.SetNomeGiocatore(messaggio.Argomento, newName);
                    _output(this, new ChatComuneMessageEventArgs(prevName + " ha cambiato il suo nome in: " + newName));
                    if (_schedeView.SelectedNode != null && _schedeView.SelectedNode.Tag is String && (_schedeView.SelectedNode.Tag as String) == messaggio.Argomento)
                        _kickButton.Text = "Espelli " + newName;
                    RefreshSchedeView(_schedeView);
                }
                if ((messaggio as GestoreGiocatoriMessageEventArgs).Messaggio is RichiestaNuovaSchedaMessageEventArgs)
                {
                    String ip = (messaggio as GestoreGiocatoriMessageEventArgs).Argomento;
                    RichiestaNuovaSchedaMessageEventArgs msg = (messaggio as GestoreGiocatoriMessageEventArgs).Messaggio as RichiestaNuovaSchedaMessageEventArgs;
                    List<String> nomi = (from Scheda s in _schede.Keys select s.IdScheda).ToList<String>();
                    nomi.AddRange(_schedePendenti.Keys);
                    if (nomi.Contains<String>(msg.Argomento))
                        _output(this, new GestoreGiocatoriMessageEventArgs(ip, new CambiaNomeSchedaMessageEventArgs(msg.Argomento, nomi)));
                    else
                    {
                        _schedePendenti.Add(msg.Argomento, ip);
                        RefreshSchedeView(_schedeView);
                    }
                }
                
            }
            else if (messaggio is RichiestaModificaCampoSchedaMessageEventArgs)
            {
                RichiestaModificaCampoSchedaMessageEventArgs msg=(messaggio as RichiestaModificaCampoSchedaMessageEventArgs);
                _values[GenerateId(msg.IdScheda, msg.IdCampo)][1] = msg.Argomento;
                if (_schedaCorrente != null) if (_schedaCorrente.IdScheda == msg.IdScheda) DisplayScheda();
                ColoraSchedeView(_schedeView.Nodes);
            }
        }

        private void InviaTemplateESchede (string ip, params String[] idSchede)
        {
            String tempString = null;
            
            IEnumerable<Scheda> schedeRichieste = from Scheda s in _schede.Keys where idSchede.Contains(s.IdScheda) select s;
            tempString = DocumentoMaster.GetIstance().Persister.FormatScheda(schedeRichieste); 
            if (tempString == null) throw new InvalidOperationException("Schede non correttamente formattate");
            _output(this, new GestoreGiocatoriMessageEventArgs(ip, new PersistanceFileSchedeMessageEventArgs(tempString)));

            Scheda[] sc = schedeRichieste.ToArray<Scheda>();
            ElementoTemplate templateRichiesto = null;
            if (_schede.Values.Count == 0) templateRichiesto = DocumentoMaster.GetIstance().Persister.LoadTemplate(DocumentoMaster.GetIstance().Path + DocumentoMaster.GetIstance().CurrentDescrittore.IdPartita + "/Template.xml");
            else templateRichiesto = sc.Length > 0 ? _schede[sc[0]] : _schede.Values.ToArray<ElementoTemplate>()[0];
           
            tempString = null;
            tempString = DocumentoMaster.GetIstance().Persister.FormatTemplate(templateRichiesto); 
            if (tempString == null) throw new InvalidOperationException("Template non correttamente formattato");
            _output(this, new GestoreGiocatoriMessageEventArgs(ip, new PersistanceFileTemplateMessageEventArgs(tempString)));


        }
    }
}
