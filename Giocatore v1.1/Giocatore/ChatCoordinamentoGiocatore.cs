using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utility.Messages;
using Utility;

namespace Giocatore
{
    public partial class ChatCoordinamentoGiocatore : UserControl, IVisualizable
    {
        protected IEnumerable<String> _testo;

        private TextBox _chatBox;

        private event Evento _output;

        private DimensionType _heightType;
        private DimensionType _widthType;
        private int _displayWidth;
        private int _displayHeight;

        #region Properties
        public int DisplayHeight
        {
            get { return _displayHeight; }
            set { _displayHeight = value; }
        }
        public int DisplayWidth
        {
            get { return _displayWidth; }
            set { _displayWidth = value; }
        }
        public DimensionType HeightType
        {
            get { return _heightType; }
            set { _heightType = value; }
        }
        public DimensionType WidthType
        {
            get { return _widthType; }
            set { _widthType = value; }
        }
        #endregion

        public ChatCoordinamentoGiocatore()
            : base()
        {
            InitializeComponent();
            _testo = new List<String>();
            _displayHeight = 200;
            _displayWidth = 50;
            _heightType = DimensionType.Pixel;
            _widthType = DimensionType.Percent;
            init();
        }

        private void init()
        {
            _chatBox = new TextBox();
            Draw();
        }

        protected void Draw()
        {
            _chatBox.Dock = DockStyle.Fill;
            _chatBox.Multiline = true;
            _chatBox.ScrollBars = ScrollBars.Vertical;
            _chatBox.ReadOnly = true;
            _chatBox.WordWrap = true;
            _chatBox.Lines = _testo.ToArray();
            _chatBox.BackColor = Color.White;

            this.Controls.Add(_chatBox);
        }

        #region iComponent
        public void Input(MessageEventArgs messaggio)
        {
            if (messaggio is ChatCoordinamentoMessageEventArgs) _chatBox.Text = messaggio.Argomento;
        }

        public event Evento Output
        {
            add { throw new NotImplementedException("l'Icomponent ChatCoordinamentoGiocatore non possiede output"); }
            remove { throw new NotImplementedException("l'Icomponent ChatCoordinamentoGiocatore non possiede output"); }
        }
        #endregion
    }
}
