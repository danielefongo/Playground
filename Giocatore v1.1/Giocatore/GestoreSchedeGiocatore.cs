using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utility;

namespace Giocatore
{
    public partial class GestoreSchedeGiocatore : GestoreSchede
    {
        List<String> _schedePendenti;
        
        public GestoreSchedeGiocatore() :base()
        {
            InitializeComponent();
            _schedePendenti = new List<string>();
        }

        protected override void init()
        {
            base.init();
            Button nuovaSchedaButton = new Button();
            nuovaSchedaButton.Text = "Richiedi nuova scheda";
            nuovaSchedaButton.Dock = DockStyle.Top;
            nuovaSchedaButton.Click += RichiediNuovaScheda;
            _splitter.Panel1.Controls.Add(nuovaSchedaButton);
        }

        public override void Carica(params String[] pathPartite)
        {
            base.Carica(pathPartite);
            RefreshSchedeView(_schedeView);
        }

        protected override void RefreshSchedeView(TreeView tree)
        {
            Scheda scheda = _schedaCorrente;
            tree.Nodes.Clear();
            foreach (Scheda s in _schede.Keys)
            {
                TreeNode node = new TreeNode();
                node.Tag = s;
                node.Text = s.IdScheda;
                tree.Nodes.Add(node);
            }
            AddSchedePendenti(tree);
            ColoraSchedeView(tree.Nodes);

            tree.ExpandAll();
            _schedaCorrente = scheda;
        }


        protected override void DisplayContenuto(Scheda scheda, TableLayoutPanel table, Contenuto con, int level)
        {
            String tabs = "";
            for (int i = 0; i < level; i++) tabs += "      ";

            String[] currentId = new String[] { scheda.IdScheda, con.Id };

            String key = con.Id;
            Label l = new Label();
            l.AutoSize = true;
            l.Text = tabs + key;
            table.Controls.Add(l);

            Label l2 = new Label();
            l2.AutoSize = true;
            l2.Text = _values[GenerateId(currentId)][0] + "";
            table.Controls.Add(l2);

            TextBox tb = new TextBox();
            tb.Text = _values[GenerateId(currentId)][1] == null ? "" : _values[GenerateId(currentId)][1];
            table.Controls.Add(tb);

            String[] ProprietaTag = new String[] { scheda.IdScheda, con.Id, scheda.GetValore(con.Id), null };
            //ID scheda | ID campo | Valore vecchio | Valore nuovo


            Button conferma = new Button();
            conferma.Tag = new Object[] { currentId, tb };
            conferma.AutoSize = true;
            if (con.ConfermaMaster)
            {
                conferma.Text = "Richiedi modifica";
                conferma.Click += RichiediModificaCampoScheda;
            }
            else
            {
                conferma.Text = "Modifica";
                conferma.Click += ModificaCampoScheda;
            }

            table.Controls.Add(conferma);

            Button prova = new Button();
            prova.AutoSize = true;
            prova.Text = "Prova";
            prova.Tag = new Object[] { currentId, con.FormulaProva };
            prova.Click += EseguiProva;
            table.Controls.Add(prova);
            if (!con.Modificabile)
            {
                tb.Enabled = false;
                conferma.Enabled = false;
            }
            if (String.IsNullOrEmpty(con.FormulaProva.Trim()))
                prova.Enabled = false;

        }
        protected override void AddSchedePendenti(TreeView tree)
        {
            foreach (String s in _schedePendenti)
            {
                Scheda pendente=null;
                TreeNode tn = new TreeNode();
                tn.BackColor = Color.Red;
                tn.Text = s;
                tn.Tag = pendente;
                tree.Nodes.Add(tn);
            }
        }

        protected virtual void RichiediNuovaScheda(object sender, EventArgs e)
        {
            String nome = "";
            if (Dialog.InputBox("Nuovo Personaggio", "Inserisci nome del nuovo personaggio", ref nome) == DialogResult.OK)
            {
                _output(this, new Utility.Messages.RichiestaNuovaSchedaMessageEventArgs(nome));
                _schedePendenti.Add(nome);
            }
            RefreshSchedeView(_schedeView);
        }
        protected virtual void CambiaNomeNuovaScheda(IEnumerable<String> forbiddenNames, String oldName)
        {
            String nome = oldName;
            _schedePendenti.Remove(oldName);
            RefreshSchedeView(_schedeView);
            while (forbiddenNames.Contains<String>(nome))
                if (Dialog.InputBox("Nuovo Personaggio", "Il nome scelto non è disponibile", ref nome) != DialogResult.OK)
                {
                    return;
                }
            _schedePendenti.Add(nome);
            _output(this, new Utility.Messages.RichiestaNuovaSchedaMessageEventArgs(nome));
            RefreshSchedeView(_schedeView);
        }
        protected virtual void NuovaSchedaConfermata(String name)
        {
            _schedePendenti.Remove(name);
            ElementoTemplate template = _schede.Values.Count > 0? _schede.Values.First<ElementoTemplate>() :   
                Documento.GetIstance().Persister.LoadTemplate(Documento.GetIstance().Path + Documento.GetIstance().CurrentDescrittore.IdPartita+ "/Template.xml");
            _schede.Add(GeneraScheda(template, name),template);
            SalvaSchede();
            InitValues();
            RefreshSchedeView(_schedeView);
        }
        protected virtual void NuovaSchedaRifiutata(String name)
        {
            _schedePendenti.Remove(name);
            if (_schedeView.SelectedNode!=null && _schedeView.SelectedNode.Text == name) _schedaPanel.Controls.Clear();
            RefreshSchedeView(_schedeView);
        }
        protected virtual void SchedaCancellata(String Id)
        {
            Scheda condannata = (from Scheda s in _schede.Keys where s.IdScheda == Id select s).First<Scheda>();
            RemoveScheda(condannata);
            if (_schedeView.SelectedNode != null && _schedeView.SelectedNode.Tag is Scheda && (_schedeView.SelectedNode.Tag as Scheda) == condannata)
                _schedaPanel.Controls.Clear();
            RefreshSchedeView(_schedeView);
            _output(this,new Utility.Messages.ChatComuneMessageEventArgs("Il personaggio "+Id+" appartenente a "+Documento.GetIstance().NomeUtente+" è morto."));
        }
        
        
        protected override void ModificaCampoScheda(object sender, EventArgs e)
        {
            String[] Id = ((sender as Control).Tag as Object[])[0] as String[];
            String newValue = (((sender as Control).Tag as Object[])[1] as TextBox).Text;
            if (String.IsNullOrEmpty(newValue)) return;
            _output(this,new Utility.Messages.ModificaCampoSchedaMessageEventArgs(Id[0], Id[1], newValue));
        }
        protected virtual void RichiediModificaCampoScheda(object sender, EventArgs e)
        {
            String[] Id = ((sender as Control).Tag as Object[])[0] as String[];
            String newValue = (((sender as Control).Tag as Object[])[1] as TextBox).Text;
            if (String.IsNullOrEmpty(newValue)) return;
            _values[GenerateId(Id)][1] = newValue;
            _output(this, new Utility.Messages.RichiestaModificaCampoSchedaMessageEventArgs(Id[0], Id[1], newValue));
        }
        protected virtual void EseguiProva(object sender, EventArgs e)
        {
            String nomeCampo = (((sender as Button).Tag as Object[])[0] as String[])[1];
            String nomePersonaggio= (((sender as Button).Tag as Object[])[0] as String[])[0];
            String testo = DocumentoGiocatore.GetIstance().NomeUtente + ": "+nomePersonaggio+" ha effettuato una prova di " + nomeCampo + ": ";
            String argomento = ((sender as Button).Tag as Object[])[1] as String;
            _output(this, new Utility.Messages.InterpretaMessageEventArgs(testo, argomento));
        }

        protected override void SwitchMessaggio(Utility.Messages.MessageEventArgs messaggio)
        {
            if (messaggio is Utility.Messages.CambiaNomeSchedaMessageEventArgs)
                CambiaNomeNuovaScheda((messaggio as Utility.Messages.CambiaNomeSchedaMessageEventArgs).Nomi, (messaggio as Utility.Messages.CambiaNomeSchedaMessageEventArgs).Argomento);
            else if (messaggio is Utility.Messages.NuovaSchedaConfermataMessageEventArgs)
                NuovaSchedaConfermata((messaggio as Utility.Messages.NuovaSchedaConfermataMessageEventArgs).Argomento);
            else if (messaggio is Utility.Messages.NuovaSchedaRifiutataMessageEventArgs)
                NuovaSchedaRifiutata((messaggio as Utility.Messages.NuovaSchedaRifiutataMessageEventArgs).Argomento);
            else if (messaggio is Utility.Messages.SchedaCancellataMessageEventArgs)
                SchedaCancellata((messaggio as Utility.Messages.SchedaCancellataMessageEventArgs).Argomento);
        }
    }
}
