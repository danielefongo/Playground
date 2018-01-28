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
    public partial class ChatCoordinamentoMaster : UserControl, IVisualizable
    {
        protected IEnumerable<String> _testo;

        private Panel _bottomPanel;
        private TextBox _chatBox;
        private Button _setButton;

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

        public ChatCoordinamentoMaster()
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
            _bottomPanel = new Panel();
            _chatBox = new TextBox();
            _setButton = new Button();
            Draw();
        }

        protected void Draw()
        {
            _chatBox.Dock = DockStyle.Fill;
            _chatBox.Multiline = true;
            _chatBox.ScrollBars = ScrollBars.Vertical;
            _chatBox.WordWrap = true;
            _chatBox.Lines = _testo.ToArray();
            _chatBox.BackColor = Color.White;

            _setButton.Text = "Conferma";
            _setButton.Dock = DockStyle.Right;
            _setButton.AutoSize = true;
            _setButton.Click += OnSet;

            _bottomPanel.Height = _setButton.Height;
            _bottomPanel.Dock = DockStyle.Bottom;

            _bottomPanel.Controls.Add(_setButton);

            this.Controls.Add(_chatBox);
            this.Controls.Add(_bottomPanel);
        }

        private void OnSet(object sender, EventArgs e)
        {
            _output(this, new ChatCoordinamentoMessageEventArgs(_chatBox.Text));
        }

        #region iComponent
        public void Input(MessageEventArgs messaggio)
        {
            ; //do nothing
        }

        public event Evento Output
        {
            add { _output += value; }
            remove { _output -= value; }
        }
        #endregion
    }
}
