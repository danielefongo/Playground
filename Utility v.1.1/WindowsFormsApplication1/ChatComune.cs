using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utility.Messages;
using System.Runtime.Serialization;

namespace Utility
{
    public partial class ChatComune : UserControl, IVisualizable
    {
        protected IEnumerable<String> _testo;

        private Panel _bottomPanel;
        private TextBox _chatBox;
        private TextBox _msgBox;
        private Button _sendButton;

        protected event Evento _output;

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

        public ChatComune() : base()
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
            _msgBox = new TextBox();
            _chatBox = new TextBox();
            _sendButton = new Button();
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

            _msgBox.Dock = DockStyle.Fill;
            _msgBox.Multiline = true;
            _msgBox.WordWrap = false;
            _msgBox.Clear();
            _msgBox.KeyDown += OnKeyDown;
            _msgBox.KeyUp += OnKeyUp;

            _sendButton.Text = "Send";
            _sendButton.Dock = DockStyle.Right;
            _sendButton.AutoSize = true;
            _sendButton.Click += OnSend;

            _bottomPanel.Height = _msgBox.Height;
            _bottomPanel.Dock = DockStyle.Bottom;

            _bottomPanel.Controls.Add(_msgBox);
            _bottomPanel.Controls.Add(_sendButton);

            this.Controls.Add(_chatBox);
            this.Controls.Add(_bottomPanel);
        }

        private void OnSend(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(_msgBox.Text))
                if (_msgBox.Text.IndexOf("/r") == 0) _output(this, new InterpretaMessageEventArgs(Documento.GetIstance().NomeUtente + " > " + _msgBox.Text.Remove(0, 2).TrimStart(), _msgBox.Text.Remove(0, 2).TrimStart()));
                else _output(this, new ChatComuneMessageEventArgs(Documento.GetIstance().NomeUtente + " > " + _msgBox.Text));
            _msgBox.Clear();

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) OnSend(sender, e);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) _msgBox.Clear();
        }
        #region iComponent
        public void Input(MessageEventArgs messaggio)
        {
            if (messaggio is ChatComuneMessageEventArgs)
            {
                ((List<String>)_testo).Add(messaggio.ToString());
                _chatBox.Lines = _testo.ToArray();
                _chatBox.SelectionStart = _chatBox.TextLength;
                _chatBox.ScrollToCaret();
            }

        }

        public event Evento Output
        {
            add { _output += value; }
            remove { _output -= value; }
        }
        #endregion
    }
}

namespace Utility.Messages
{
    [Serializable]
    public class ChatComuneMessageEventArgs : MessageEventArgs
    {
        public ChatComuneMessageEventArgs(String argomento) : base(argomento) { }
        public ChatComuneMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}