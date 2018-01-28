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
    public partial class MainFormGiocatore : MainForm
    {
        private ToolStripMenuItem _personaggiMenuItem;
        public MainFormGiocatore()
        {
            InitializeComponent();
            base._nuovaPartitaToolStripMenuItem.Text = "Carica Partita";
            base._nuovaPartitaToolStripMenuItem.Enabled = false;

            _personaggiMenuItem = new ToolStripMenuItem();
            _personaggiMenuItem.Text = "Template";
            _personaggiMenuItem.Enabled = false;
            _partitaToolStripMenuItem.DropDownItems.Add(_personaggiMenuItem);
            
        }
        protected override void impostazioniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newName = DocumentoGiocatore.GetIstance().NomeUtente;
            if (Dialog.InputBox("Impostazioni", "Inserisci il tuo nome utente", ref newName) == DialogResult.OK) DocumentoGiocatore.GetIstance().NomeUtente = newName;
        }

    }
}
