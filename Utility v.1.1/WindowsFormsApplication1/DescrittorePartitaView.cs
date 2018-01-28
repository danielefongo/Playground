using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utility
{
    public partial class DescrittorePartitaView : UserControl
    {
        private DescrittorePartita _descrittore;

        public DescrittorePartita Descrittore
        {
            get { return _descrittore; }
            set 
            { 
                _descrittore = value; _nomePartitaLabel.Text = _descrittore.Nome; _nomeMasterLabel.Text = _descrittore.NomeMaster;
                if (_descrittore.Stato == StatoPartita.Inattiva) BackColor = Color.Yellow;
                else if (_descrittore.Stato == StatoPartita.Terminata) BackColor = Color.Red;
                else BackColor = Color.White;
            }
        }

        public DescrittorePartitaView()
        {
            InitializeComponent();
        }

        public DescrittorePartitaView(DescrittorePartita desc) : this () {Descrittore = desc;}

        private void _nomePartitaLabel_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void _nomeMasterLabel_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        public void Aggiorna()
        {
            _nomePartitaLabel.Text = _descrittore.Nome; 
            _nomeMasterLabel.Text = _descrittore.NomeMaster;
        }
    }
}
