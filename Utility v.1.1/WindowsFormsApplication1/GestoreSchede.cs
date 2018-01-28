using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using Utility.Messages;

namespace Utility
{
    public partial class GestoreSchede : UserControl, IVisualizable
    {
        protected Evento _output;
        protected Scheda _schedaCorrente;
        

        //persistence variables
        protected Dictionary<Scheda, ElementoTemplate> _schede;
        protected Dictionary<String, String[]> _values;        

        //graphic variables
        protected TreeView _schedeView;
        protected Panel _schedaPanel;
        protected SplitContainer _splitter;

        //properties
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

        public GestoreSchede()
        {
            _schede = new Dictionary<Scheda,ElementoTemplate>();
            _values = new Dictionary<String, String[]>();
            _heightType = DimensionType.Pixel;
            _widthType = DimensionType.Percent;
            _displayHeight = 300;

            _displayWidth = 100;

            InitializeComponent();
            init();
        }

        //GESTIONE GRAFICA
        #region Grafica
        protected virtual void init()
        {
            _splitter = new SplitContainer();
            _splitter.Orientation = Orientation.Vertical;
            _splitter.Dock = DockStyle.Fill;
            _splitter.SplitterDistance = _splitter.Size.Width / 4;

            _schedeView = new TreeView();
            _schedaPanel = _splitter.Panel2;
            _schedeView.Nodes.Add(new TreeNode());

            _schedeView.AfterSelect += OnSchedaSelected;
            _schedeView.Dock = DockStyle.Fill;
            _schedeView.HideSelection = false;
            _splitter.Panel1.Controls.Add(_schedeView);

            _schedaPanel.BorderStyle = BorderStyle.FixedSingle;

            this.Controls.Add(_splitter);
        }

        protected void InitValues()
        {
            foreach (Scheda s in _schede.Keys)
                foreach (String id in s.GetIds())
                {
                    String key = GenerateId(new String[2]{s.IdScheda,id});
                    String[] value = new String[2] { s.GetValore(id), null };
                    if(!_values.Keys.Contains(key))
                        _values.Add(key, value);
                }
        }

        protected void RemoveScheda(Scheda scheda)
        {
            _schede.Remove(scheda);
            foreach (String id in scheda.GetIds())
            {
                String key = GenerateId(scheda.IdScheda, id);
                if (_values.Keys.Contains(key)) _values.Remove(key);
            }
        }

        protected virtual void RefreshSchedeView(TreeView tree)
        {
            Scheda scheda = _schedaCorrente;
            tree.Nodes.Clear();
            foreach(Scheda s in _schede.Keys)
            {
                TreeNode parentnode = null;
                foreach(TreeNode t in tree.Nodes)
                {
                    if (t.Tag == _schede[s]) parentnode = t;
                }
                if(parentnode==null)
                {
                    parentnode = new TreeNode();
                    parentnode.Tag = _schede[s];
                    parentnode.Text = _schede[s].Id;
                    tree.Nodes.Add(parentnode);
                }
                TreeNode node = new TreeNode();
                node.Tag = s;
                node.Text = s.IdScheda;
                parentnode.Nodes.Add(node);
            }
            AddSchedePendenti(tree);
            tree.ExpandAll();
            _schedaCorrente = scheda;
            
            this.ColoraSchedeView(_schedeView.Nodes);
        }

        protected virtual void AddSchedePendenti(TreeView tree)
        {
            ; //do nothing
        }

        protected Boolean ColoraSchedeView(TreeNodeCollection nodes)
        {
            Boolean result = false;
            foreach (TreeNode tn in nodes)
            {
                if (tn.Tag is String)
                    if (ColoraSchedeView(tn.Nodes))
                    {
                        tn.BackColor = Color.Yellow;
                        result = true;
                    }
                    else
                    {
                        tn.BackColor = Color.Empty;
                        result = false;
                    }
                else if (tn.Tag is Scheda)
                {
                    Scheda s = tn.Tag as Scheda;
                    foreach (String id in s.GetIds())
                        if (_values[GenerateId(s.IdScheda, id)][1] != null)
                        {
                            tn.BackColor = Color.Yellow;
                            result = true;
                        }

                    if (result == false) tn.BackColor = Color.Empty;
                }
                else if (tn.Nodes.Count > 0)
                    result = ColoraSchedeView(tn.Nodes);
                else result = false;
            }
            return result;
        }

        private void OnSchedaSelected(object sender, EventArgs e)
        {
            
            if (((TreeView)sender).SelectedNode.Tag is Scheda && ((TreeView)sender).SelectedNode.Tag != null)
            {
                _schedaCorrente = (Scheda)((TreeView)sender).SelectedNode.Tag;
                DisplayScheda();
            }
            else
            {
                _schedaPanel.Controls.Clear();
                _schedaCorrente = null;
                OnElementSelected(sender, e);
            }
        }

        protected virtual void OnElementSelected(object sender, EventArgs e)
        {
            ; //pattern template
        }
        
        protected virtual void DisplayScheda()
        {
            if (_schedaCorrente == null) return;
            _schedaPanel.Controls.Clear();
            Panel spacer = new Panel();
            spacer.Dock = DockStyle.Top;
            spacer.Height = 10;
            TableLayoutPanel table = new TableLayoutPanel();
            table.Dock = DockStyle.Fill;
            table.ColumnCount = 5; 
            table.AutoScroll = true;
            if (_schedaCorrente == null) return; 
            DisplaySchedaRecursion(_schedaCorrente, table, _schede[_schedaCorrente], 0);
           
            _schedaPanel.Controls.Add(table); 
            _schedaPanel.Controls.Add(spacer);
            
        }

        protected void DisplaySchedaRecursion(Scheda scheda, TableLayoutPanel table, ElementoTemplate et, int level)
        {
            if(level < 0)level=0;
            if (_schedaCorrente == null) return;
            String tabs = "";
            for (int i = 0; i < level; i++) tabs += "      ";

            if (et is Contenitore)
            {

                Label l = new Label();
                l.AutoSize = true;
                l.Text = tabs+((Contenitore)et).Id;
                table.Controls.Add(l);
                for (int i = 0; i < table.ColumnCount - 1; i++)table.Controls.Add(new Label());

                foreach (ElementoTemplate el in ((Contenitore)et).Children)DisplaySchedaRecursion(scheda, table, el, level + 1);
            }
            else if (et is Contenuto)
            {
                Contenuto con = (Contenuto)et;
                DisplayContenuto(scheda,table,con,level);
            }
        }

        protected virtual void DisplayContenuto(Scheda scheda, TableLayoutPanel table, Contenuto con,int level) 
        {
            if (_schedaCorrente == null) return;
            String tabs = "";
            for (int i = 0; i < level; i++) tabs += "      ";

            String key = con.Id;
            Label l = new Label();
            l.AutoSize = true;
            l.Text = tabs + key;
            table.Controls.Add(l);

            Label l2 = new Label();
            l2.AutoSize = true;
            l2.Text = scheda.GetValore(key);
            table.Controls.Add(l2);

            TextBox tb = new TextBox();
            tb.Text = scheda.GetValore(key);
            table.Controls.Add(tb);

            String[] ProprietaTag = new String[] { scheda.IdScheda, con.Id, scheda.GetValore(con.Id), null };
            //ID scheda | ID campo | Valore vecchio | Valore nuovo

            Button conferma = new Button();
            conferma.Text = "Conferma";
            conferma.Tag = ProprietaTag;
            conferma.Click += ModificaCampoScheda;
            table.Controls.Add(conferma);

            Button cancella = new Button();
            cancella.Text = "Cancella";
            cancella.Tag = con; 
            cancella.Click += RipristinaCampoScheda;
            table.Controls.Add(cancella);
        }


        protected virtual void ModificaCampoScheda(object sender, EventArgs e)
        {
            ; //do nothing
        }
        protected virtual void RipristinaCampoScheda(object sender, EventArgs e)
        {
            ; //do nothing
        }
        #endregion

        //GESTIONE COMPUTAZIONALE
        #region Computazione

        public void Parsifica(String template, params String[] schede)
        {
            if (String.IsNullOrEmpty(template) || schede.Length == 0) throw new ArgumentException("Parametri errati");
            ElementoTemplate t=ParsificaTemplate(template);
            foreach (Scheda s in ParsificaSchede(schede)) _schede.Add(s, t);
        }
        protected ElementoTemplate ParsificaTemplate(String template)
        {
            return Documento.GetIstance().Persister.FormatTemplate(template);
        }
        protected IEnumerable<Scheda> ParsificaSchede(params String[] schede)
        {
            List<Scheda> result= new List<Scheda>();
            foreach (String scheda in schede)result.AddRange(Documento.GetIstance().Persister.FormatScheda(scheda));
            return result;
        }

        public virtual void Carica(params String[] pathPartite) 
        {
            if (pathPartite.Length == 0) throw new ArgumentException("Parametri errati");
            bool valid;
            foreach (String path in pathPartite)
            {
                ElementoTemplate root = CaricaTemplate(path);
                foreach (Scheda s in CaricaSchede(path))
                {
                    valid=true;
                    //if (IsSchedaErrata(root, s)) throw new InvalidDataException("Scheda errata");
                    foreach (Scheda old in _schede.Keys)
                    {
                        if (old.IdScheda == s.IdScheda && ((Contenitore)_schede[old]).Id == ((Contenitore)root).Id) valid = false;
                            //throw new InvalidDataException("Scheda già presente");
                    }
                    if (!IsSchedaErrata(root, s) && valid) _schede.Add(s, root);
                }
            }
            InitValues();
        }

        protected ElementoTemplate CaricaTemplate(String path)
        {
            if (path == null) throw new InvalidDataException();
            //if (!Directory.Exists(path)) throw new IOException();
            //if (!File.Exists(path + "/Template.xml")) throw new IOException();
            ElementoTemplate root = new Contenitore("root");
            try { root = Documento.GetIstance().Persister.LoadTemplate(path + "/Template.xml"); }
            catch { Documento.GetIstance().Persister.SaveTemplate(path + "/Template.xml", root); }
            return root;
        }

        protected IEnumerable<Scheda> CaricaSchede(String path)
        {
            if (path == null) throw new InvalidDataException();
            //if (!Directory.Exists(path)) throw new IOException();
            //if (!File.Exists(path + "/Schede.xml")) throw new IOException();
            IEnumerable<Scheda> schede = new List<Scheda>(); 
            try { schede = Documento.GetIstance().Persister.LoadScheda(path + "/Schede.xml"); }
            catch { Documento.GetIstance().Persister.SaveScheda(path + "/Schede.xml", schede); }
            return schede;
        }

        public void SalvaSchede()
        {
            Documento.GetIstance().Persister.SaveScheda(Documento.GetIstance().Path + Documento.GetIstance().CurrentDescrittore.IdPartita + "/Schede.xml", _schede.Keys);
        }
        public virtual void Salva(object sender, EventArgs e)
        {
            SalvaSchede();
        }

        protected Scheda GeneraScheda(ElementoTemplate e, String nome)
        {
            Scheda s = new Scheda(nome);
            GeneraSchedaRecursion(e, s);
            return s;
        }

        private void GeneraSchedaRecursion(ElementoTemplate e, Scheda s)
        {
            if (e is Contenitore)
                foreach (ElementoTemplate figlio in ((Contenitore)e).Children)
                {
                    GeneraSchedaRecursion(figlio, s);
                }
            else if (e is Contenuto) s.SetValore(e.Id, "");
        }

        private bool IsSchedaErrata(ElementoTemplate e, Scheda s) 
        {
            bool result = false;
            if (e is Contenitore)
                foreach (ElementoTemplate figlio in ((Contenitore)e).Children)
                {
                    if (IsSchedaErrata(figlio, s)) { return true; }
                }
            else if (e is Contenuto && !s.GetIds().Contains<String>(e.Id)) return true;
            return result;
        }

        protected void ModificaCampoScheda(string idScheda, string idCampo, string valore)
        {
            bool isValid = true;
            int defaultInt=default(int);
            Scheda scheda=(from s in _schede.Keys where s.IdScheda == idScheda select s).ElementAt(0);
            Contenuto et= GetContenutoById(_schede[scheda],idCampo);
            if (et == null) isValid = false;
            switch(et.Tipo)
            {
                case TipoValore.Numero :
                    if (String.IsNullOrEmpty(valore)) valore = defaultInt + "";
                    else if (!Int32.TryParse(valore, out defaultInt)) isValid = false;
                    break;
                case TipoValore.Testo : break;
                default: isValid = false; break;
            }

            if (isValid)
            {
                scheda.SetValore(idCampo, valore);
                _values[GenerateId(idScheda, idCampo)][0] = valore;
            }
            _values[GenerateId(idScheda, idCampo)][1] = null;
            if (_schedaCorrente!=null) if(_schedaCorrente.IdScheda == idScheda) DisplayScheda();
            this.ColoraSchedeView(_schedeView.Nodes);
        }

        public void Clear()
        {
            _schedaPanel.Controls.Clear();
            _schedeView.Nodes.Clear();
            _schede.Clear();
        }
        #endregion


        protected Contenuto GetContenutoById(ElementoTemplate root,string idCampo)
        {
            if (root.Id == idCampo && root is Contenuto) return root as Contenuto;
            else if (root is Contenitore)
            {
                foreach (ElementoTemplate et in (root as Contenitore).Children)
                {
                    Contenuto result = GetContenutoById(et, idCampo);
                    if (result != null) return result;
                }
            }
            return null;
        }

        //GESTIONE INTERPRETE
        #region Interprete
        private int Evaluate(string expression)
        {
            var loDataTable = new DataTable();
            var loDataColumn = new DataColumn("Eval", typeof(double), expression);
            loDataTable.Columns.Add(loDataColumn);
            loDataTable.Rows.Add(0);
            return Convert.ToInt32((double)(loDataTable.Rows[0]["Eval"]));
        }

        private String Replace(ElementoTemplate e, String expression)
        {
            if (e is Contenitore)
            {
                foreach (ElementoTemplate figlio in ((Contenitore)e).Children)
                {
                    expression = Replace(figlio, expression);
                }
            }
            else
            {
                String nomecontenuto = ((Contenuto)e).Id;
                String valore = _schedaCorrente.GetValore(nomecontenuto);
                expression = expression.Replace(nomecontenuto, valore);
            }
            return expression;
        }

        public String Interpreta(string expression)
        {
            if (_schedaCorrente != null) expression = Replace(_schede[_schedaCorrente], expression);

            Random random = new Random();
            int j, k, risultato;
            String prev = "";
            String next = "";
            String eval = "";
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == 'd')
                {
                    next = "";
                    prev = "";
                    j = i + 1;
                    k = i - 1;
                    risultato = 0;

                    while (j < expression.Length && ((int)expression[j]) < 58 && ((int)expression[j]) > 47) //calcola il next (sfrutto la codifica ascii per verificare se è un numero)
                    {
                        next = next + expression[j];
                        j++;
                    }
                    while (k >= 0 && ((int)expression[k]) < 58 && ((int)expression[k]) > 47) //calcola il prev (sfrutto la codifica ascii per verificare se è un numero)
                    {
                        prev = expression[k] + prev;
                        k--;
                    }
                    if (next.Length > 0 && prev.Length > 0)
                    {
                        for (int count = 0; count < Int32.Parse(prev); count++)
                        {
                            risultato += random.Next(Int32.Parse(next)) + 1;
                        }
                        expression = expression.Replace(prev + "d" + next, risultato + "");
                    }
                    else if (next.Length > 0 && prev.Length == 0)
                    {
                        risultato += random.Next(Int32.Parse(next)) + 1;
                        expression = expression.Replace("d" + next, risultato + "");
                    }
                }
            }

            for (int i = 0; i < expression.Length; i++)
            {
                if ((int)expression[i] > 39 && (int)expression[i] < 58 && (int)expression[i] != 44 || (int)expression[i] == 32)
                {
                    eval += expression[i];
                    
                }
                else if (eval.Length != 0 && !String.IsNullOrWhiteSpace(eval))
                {
                    expression = expression.Replace(eval, ""+Evaluate(eval));
                    eval = "";
                }
            }
            if (eval.Length != 0 && !String.IsNullOrWhiteSpace(eval)) expression = expression.Replace(eval, "" + Evaluate(eval));
            return expression;
        }
        #endregion

        //GESTIONE MESSAGGISTICA
        #region Messaggistica
        public void Input(MessageEventArgs messaggio)
        {
            if (messaggio is InterpretaMessageEventArgs)
            {
                String valore = "Errore di interpretazione";
                try
                {
                    valore = Interpreta(messaggio.Argomento);
                }
                catch { ;}
                _output(this, new ChatComuneMessageEventArgs(((InterpretaMessageEventArgs)messaggio).Testo + " = " + valore));
            }
            else if (messaggio is RichiestaModificaCampoSchedaMessageEventArgs)
            {
                SwitchMessaggio(messaggio);
            }
            else if (messaggio is ModificaCampoSchedaMessageEventArgs)
            {
                ModificaCampoScheda(((ModificaCampoSchedaMessageEventArgs)messaggio).IdScheda, ((ModificaCampoSchedaMessageEventArgs)messaggio).IdCampo, ((ModificaCampoSchedaMessageEventArgs)messaggio).Argomento);
            }
            else 
            {
                SwitchMessaggio(messaggio);
            }
            
        }
        protected String GenerateId(String[] par)
        {
            return GenerateId(par[0],par[1]);
        }
        protected String GenerateId(String par1, String par2)
        {
            return par1 + "§" + par2;
        }

        public event Evento Output
        {
            add { _output += value; }
            remove { _output -= value; }
        }

        protected virtual void SwitchMessaggio(MessageEventArgs messaggio)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}

namespace Utility.Messages
{
    [Serializable]
    public class InterpretaMessageEventArgs : MessageEventArgs
    {
        private readonly String _testo;

        public String Testo
        {
            get { return _testo; }
        }

        public override String ToString() { return _testo + " = " + this.Argomento; }

        public InterpretaMessageEventArgs(String argomento) : base(argomento) { _testo = ""; }
        public InterpretaMessageEventArgs(String testo, String argomento) : base(argomento) { _testo = testo; }
        public InterpretaMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class PersistanceFileSchedeMessageEventArgs : MessageEventArgs
    {
        public PersistanceFileSchedeMessageEventArgs(String argomento) : base(argomento) { }
        public PersistanceFileSchedeMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class PersistanceFileTemplateMessageEventArgs : MessageEventArgs
    {
        public PersistanceFileTemplateMessageEventArgs(String argomento) : base(argomento) { }
        public PersistanceFileTemplateMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class PersistanceErrorMessageEventArgs : MessageEventArgs
    {
        public PersistanceErrorMessageEventArgs(String argomento) : base(argomento) { }
        public PersistanceErrorMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class ModificaCampoSchedaMessageEventArgs : MessageEventArgs, ISerializable
    {
        private readonly String _idScheda;
        private readonly String _idCampo;

        public String IdScheda { get { return _idScheda; } }
        public String IdCampo { get { return _idCampo; } } 

        public override String ToString() { return _idScheda + "§" + _idCampo + "§" + this.Argomento; } 

        public ModificaCampoSchedaMessageEventArgs(String idScheda, String idCampo, String argomento) : base(argomento) { _idScheda = idScheda; _idCampo = idCampo; }
        public ModificaCampoSchedaMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context)
        { 
            _idScheda = (String)info.GetValue("idScheda", typeof(String));
            _idCampo = (String)info.GetValue("idCampo", typeof(String));
        }
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("message", _argomento, typeof(String));
            info.AddValue("idScheda", _idScheda, typeof(String));
            info.AddValue("idCampo", _idCampo, typeof(String));
        }
    }

    [Serializable]
    public class RichiestaModificaCampoSchedaMessageEventArgs : ModificaCampoSchedaMessageEventArgs
    {
        public RichiestaModificaCampoSchedaMessageEventArgs (String idScheda, String idCampo, String argomento) : base(idScheda, idCampo, argomento) {}
        public RichiestaModificaCampoSchedaMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class SchedaCancellataMessageEventArgs : MessageEventArgs
    {
        public SchedaCancellataMessageEventArgs(String argomento) : base(argomento) { }
        public SchedaCancellataMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class RichiestaNuovaSchedaMessageEventArgs : MessageEventArgs
    {
        public RichiestaNuovaSchedaMessageEventArgs(String argomento) : base(argomento) { }
        public RichiestaNuovaSchedaMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class CambiaNomeSchedaMessageEventArgs : MessageEventArgs, ISerializable
    {
        private readonly List<String> _nomi;

        public List<String> Nomi { get { return _nomi; } }

        public override String ToString()
        {
            String temp = this.Argomento + ":";
            foreach (String n in _nomi)
            {
                temp = temp + n + "§";
            }
            return temp;
        }

        public CambiaNomeSchedaMessageEventArgs(String argomento, List<String> nomi) : base(argomento) { _nomi = nomi; }
        public CambiaNomeSchedaMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _nomi = (List<String>)info.GetValue("nomi", typeof(List<String>));
        }
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("message", _argomento, typeof(String));
            info.AddValue("nomi", _nomi, typeof(List<String>));
        }
    }

    [Serializable]
    public class NuovaSchedaRifiutataMessageEventArgs : MessageEventArgs
    {
        public NuovaSchedaRifiutataMessageEventArgs(String argomento) : base(argomento) { }
        public NuovaSchedaRifiutataMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class NuovaSchedaConfermataMessageEventArgs : MessageEventArgs
    {
        public NuovaSchedaConfermataMessageEventArgs(String argomento) : base(argomento) { }
        public NuovaSchedaConfermataMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class CreaNuovaSchedaMessageEventArgs : MessageEventArgs
    {
        public CreaNuovaSchedaMessageEventArgs(String argomento) : base(argomento) { }
        public CreaNuovaSchedaMessageEventArgs(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}