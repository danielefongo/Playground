using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utility;

namespace Master
{
    public partial class MainFormMaster : MainForm
    {
        private ToolStripMenuItem _templateMenuItem;
        private ToolStripMenuItem _cercaTemplateMenuItem;
        private ToolStripMenuItem _nuovoTemplateMenuItem;
        private ToolStripMenuItem _modificaPartitaMenuItem;
        
        public MainFormMaster() : base()
        {
            InitializeComponent();
            #region Menu
            _templateMenuItem = new ToolStripMenuItem();
            _templateMenuItem.Text = "Template";
            _cercaTemplateMenuItem = new ToolStripMenuItem();
            _cercaTemplateMenuItem.Text = "Cerca Template";
            _cercaTemplateMenuItem.Enabled = false;
            _nuovoTemplateMenuItem = new ToolStripMenuItem();
            _nuovoTemplateMenuItem.Text = "Nuovo Template";
            _nuovoTemplateMenuItem.Enabled = false;
            _modificaPartitaMenuItem = new ToolStripMenuItem();
            _modificaPartitaMenuItem.Text = "Modifica Partita";
            _modificaPartitaMenuItem.Click += ModificaPartita;
            _templateMenuItem.DropDownItems.Add(_nuovoTemplateMenuItem);
            _templateMenuItem.DropDownItems.Add(_cercaTemplateMenuItem);
            base._menu.Items.Add(_templateMenuItem);
            base._partitaToolStripMenuItem.DropDownItems.Add(_modificaPartitaMenuItem);
            #endregion
        }

        private void ModificaPartita(object o, EventArgs e)
        {
            if (_currentDescrittoreView != null)
                if ((DescrittorePartitaModifier.GetInputBox(_currentDescrittoreView.Descrittore)) == DialogResult.OK)
                {
                    
                    DocumentoMaster.GetIstance().Persister.SaveDescrittorePartita(DocumentoMaster.GetIstance().Path + "Descrittori.xml", Descrittori);
                    this.Aggiorna();
                }
        }

        protected override void _joinButton_Click(object sender, EventArgs e)
        {
            if (_currentDescrittoreView != null) DocumentoMaster.GetIstance().JoinPartita(_currentDescrittoreView.Descrittore);
        }

        protected override void joinPartitaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentDescrittoreView != null) DocumentoMaster.GetIstance().JoinPartita(_currentDescrittoreView.Descrittore);
        }

        private void MainFormMaster_FormClosed(object sender, FormClosedEventArgs e)
        {
            DocumentoMaster.GetIstance().CloseApplication();
        }

        protected override void impostazioniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newName = DocumentoMaster.GetIstance().NomeUtente;
            if (Dialog.InputBox("Impostazioni", "Inserisci il tuo nome utente", ref newName) == DialogResult.OK) DocumentoMaster.GetIstance().NomeUtente = newName;
        }

        
    }
}
