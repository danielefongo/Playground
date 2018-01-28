using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utility
{
    public partial class MainForm : Form
    {
        protected DescrittorePartitaView _currentDescrittoreView;
        private bool _allowJoin;

        public bool AllowJoin
        {
            get { return _allowJoin; }
            set 
            {
                if (_currentDescrittoreView.Descrittore.Stato == StatoPartita.Terminata) _joinButton.Enabled = false;
                else _joinButton.Enabled = value;
                _allowJoin = value;
            }
        }

        public DescrittorePartita CurrentDescrittore
        {
            get { return _currentDescrittoreView.Descrittore; }
        }

        public IEnumerable<DescrittorePartita> Descrittori
        {
            get 
            {
                try { return (from descrittori in _descrittoriFlowLayoutPanel.Controls.OfType<DescrittorePartitaView>() select descrittori.Descrittore); }
                catch { return new List<DescrittorePartita>(); }
            }
            set 
            { 
                _descrittoriFlowLayoutPanel.Controls.Clear();
                _descrittoriFlowLayoutPanel.Controls.AddRange((from d in value select new DescrittorePartitaView(d)).ToArray());
                foreach (Control c in _descrittoriFlowLayoutPanel.Controls) c.Click += PartitaSelezionata;
                if (_currentDescrittoreView != null) PartitaSelezionata(_currentDescrittoreView, new EventArgs());
            }
        }

        public MainForm()
        {
            InitializeComponent();
            _allowJoin = true;
        }

        protected void Aggiorna()
        {
            _currentDescrittoreView.Aggiorna();
            _descrizioneText.Text = _currentDescrittoreView.Descrittore.Descrizione;
        }

        protected virtual void PartitaSelezionata(object sender, EventArgs e)
        {
            if (sender is DescrittorePartitaView) 
            {
                if (_currentDescrittoreView != null) _currentDescrittoreView.BorderStyle = BorderStyle.None;
                _currentDescrittoreView = (DescrittorePartitaView)sender;
                _currentDescrittoreView.BorderStyle = BorderStyle.FixedSingle;
                _descrizioneText.Text = _currentDescrittoreView.Descrittore.Descrizione;
                if (_currentDescrittoreView.Descrittore.Stato == StatoPartita.Terminata) { _joinButton.Enabled = false; _joinPartitaToolStripMenuItem.Enabled = false; }
                else { _joinButton.Enabled = _allowJoin; _joinPartitaToolStripMenuItem.Enabled = _allowJoin; }
            }

        }

        protected virtual void _joinButton_Click(object sender, EventArgs e)
        {
            if (_currentDescrittoreView != null) Documento.GetIstance().JoinPartita(_currentDescrittoreView.Descrittore);
        }

        protected virtual void joinPartitaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentDescrittoreView != null) Documento.GetIstance().JoinPartita(_currentDescrittoreView.Descrittore);
        }

        protected void _descrittoriFlowLayoutPanel_Resize(object sender, EventArgs e)
        {
            foreach (Control c in _descrittoriFlowLayoutPanel.Controls)
            {
                if (c is DescrittorePartitaView)
                {
                    c.Width = c.Parent.Width - 30;
                }
            }
        }

        protected virtual void impostazioniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newName = Documento.GetIstance().NomeUtente;
            if (Dialog.InputBox("Impostazioni", "Inserisci il tuo nome utente", ref newName) == DialogResult.OK) Documento.GetIstance().NomeUtente = newName;
        }

    }
}
