using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utility;

namespace Master
{
    public partial class DescrittorePartitaModifier : Form
    {
        private static DescrittorePartita _descrittore;
        private static DescrittorePartitaModifier _descrittorePartitaModifier;


        private DescrittorePartitaModifier(DescrittorePartita descrittore)
        {
            InitializeComponent();
            this.AcceptButton = _ok;
            this.CancelButton = _annulla;
            _ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            _annulla.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            
        }

        public static DialogResult GetInputBox(DescrittorePartita descrittore)
        {
            DialogResult result;
            if(descrittore != null) _descrittore = descrittore;
            else return DialogResult.Abort;

            if(_descrittorePartitaModifier == null) _descrittorePartitaModifier = new DescrittorePartitaModifier(descrittore);
            _descrittorePartitaModifier._nome.Text = _descrittore.Nome;
            _descrittorePartitaModifier._nomeMaster.Text = _descrittore.NomeMaster;
            _descrittorePartitaModifier._password.Text = _descrittore.Password;
            _descrittorePartitaModifier._descrizione.Text = _descrittore.Descrizione;
            result = _descrittorePartitaModifier.ShowDialog();
            return result;

        }

        private void _ok_Click(object sender, EventArgs e)
        {
            _descrittore.Nome = _nome.Text;
            _descrittore.NomeMaster = _nomeMaster.Text;
            _descrittore.Password = _password.Text;
            _descrittore.Descrizione = _descrizione.Text;
        }

    }
}
