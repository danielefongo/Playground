using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Utility
{
    public class XmlPersister : IPersister
    {
        private XmlDocument _xmlDocument;
        private XmlWriter _writer;
        private Encoding _encoding;

        public XmlPersister()
        {
            _encoding =  Encoding.UTF8;
            _xmlDocument = new XmlDocument();
        }

        private void CreateFileInFolder(String path)
        {
            FileInfo file = new FileInfo(path);
            DirectoryInfo dir = null;
            try { dir = new DirectoryInfo(Path.GetDirectoryName(path)); }
            catch { };
            if(dir!=null&&!dir.Exists) dir.Create();
            if (!file.Exists) file.Create().Dispose();
        }

        #region Template
        ElementoTemplate IPersister.LoadTemplate(string path)
        {
            _xmlDocument.Load(path);
            return ReadTemplate();
        }
        ElementoTemplate IPersister.FormatTemplate(string XML) 
        {
            _xmlDocument.LoadXml(XML);
            return ReadTemplate();
            
        }
        private ElementoTemplate ReadTemplate()
        {
            XmlElement rootElement = (XmlElement)_xmlDocument.SelectSingleNode("Template");
            if (rootElement == null) throw new IOException("Invalid Format Exception");
            rootElement = (XmlElement)rootElement.FirstChild;


            if (rootElement != null && rootElement.LocalName == "Contenitore")
            {
                return LoadContenitore(rootElement);
            }
            else new IOException();
            return null;
        }

        private ElementoTemplate LoadContenitore(XmlElement contenitore)
        {
            string nome = contenitore.GetAttribute("name");
            Contenitore parent = new Contenitore(nome);
            foreach (XmlElement child in contenitore.ChildNodes)
            {
                if (child.LocalName == "Contenitore") LoadContenitore(child).Parent = parent;
                else if (child.LocalName == "Contenuto") LoadContenuto(child).Parent = parent;
                else throw new IOException();
            }
            return parent;
        }

        private ElementoTemplate LoadContenuto(XmlElement contenuto)
        {
            string nome = contenuto.GetAttribute("name");
            string tipo = contenuto.GetAttribute("tipo");
            string formulaProva = contenuto.GetAttribute("formula");
            bool modificabile = contenuto.HasAttribute("modificabile");
            bool confermaMaster = contenuto.HasAttribute("conferma");
            bool identificatorePersonaggio = contenuto.HasAttribute("identificatore");
            TipoValore tipoValore;
            try { tipoValore = (TipoValore)Enum.Parse(typeof(TipoValore), tipo); }
            catch { tipoValore = TipoValore.Testo; }
            ElementoTemplate child = new Contenuto(nome, tipoValore, formulaProva, modificabile, confermaMaster, identificatorePersonaggio);
            return child;
        }


        void IPersister.SaveTemplate(string path, ElementoTemplate root)
        {
            CreateFileInFolder(path);
            using (_writer = XmlWriter.Create(path, new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 }))
            {
                WriteTemplate(root);
            }
        }
        String IPersister.FormatTemplate(ElementoTemplate root)
        {
            using (StringWriter stringWriter = new StringWriterWithEncoding(_encoding))
            {
                using (_writer = XmlWriter.Create(stringWriter, new XmlWriterSettings() { Indent = true, Encoding=Encoding.UTF8 }))
                {
                    WriteTemplate(root);
                }
                return stringWriter.ToString();
            }
        }
        private void WriteTemplate(ElementoTemplate root)
        {
            _writer.WriteStartDocument();
            _writer.WriteStartElement("Template");
            if (root is Contenitore) SaveContenitore((Contenitore)root);
            else throw new IOException();
            _writer.WriteEndElement();
            _writer.WriteEndDocument();
        }


        private void SaveContenitore(Contenitore contenitore)
        {
            _writer.WriteStartElement("Contenitore");
            _writer.WriteAttributeString("name", contenitore.Id);
            foreach (ElementoTemplate child in contenitore.Children)
            {
                if (child is Contenuto)  SaveContenuto((Contenuto) child);
                else if (child is Contenitore)  SaveContenitore((Contenitore) child);
            }
            _writer.WriteEndElement();  
        }

        private void SaveContenuto(Contenuto contenuto)
        {

            _writer.WriteStartElement("Contenuto");
            _writer.WriteAttributeString("name", contenuto.Id);
            _writer.WriteAttributeString("tipo", contenuto.Tipo.ToString());
            _writer.WriteAttributeString("formula", contenuto.FormulaProva);
            if (contenuto.Modificabile) _writer.WriteAttributeString("modificabile", "");
            if (contenuto.ConfermaMaster) _writer.WriteAttributeString("conferma", "");
            if (contenuto.IdentificatorePersonaggio) _writer.WriteAttributeString("identificatore", "");
            _writer.WriteEndElement(); 
        }
        #endregion

        #region Scheda
        IEnumerable<Scheda> IPersister.LoadScheda(string path)
        {
            _xmlDocument.Load(path);
            return ReadScheda();
        }
        IEnumerable<Scheda> IPersister.FormatScheda(string Xml)
        {
            _xmlDocument.LoadXml(Xml);
            return ReadScheda();
        }
        private IEnumerable<Scheda> ReadScheda()
        {
            List<Scheda> schede = new List<Scheda>();
            XmlElement rootElement = (XmlElement)_xmlDocument.SelectSingleNode("Schede");
            if (rootElement == null) throw new IOException("Invalid Format Exception");

            foreach (XmlNode elemento in rootElement.ChildNodes)
            {
                if (elemento.LocalName != "Scheda") throw new IOException("Invalid Format Exception - Scheda non valida");
                Scheda s = new Scheda(((XmlElement)elemento).GetAttribute("name"));
                foreach (XmlNode campo in elemento.ChildNodes)
                {
                    if (campo.LocalName != "Campo") throw new IOException("Invalid Format Exception - Campo non trovato");
                    s.SetValore(((XmlElement)campo).GetAttribute("name"), ((XmlElement)campo).GetAttribute("valore"));
                }
                schede.Add(s);
            }

            return schede;
        }

        void IPersister.SaveScheda(string path, IEnumerable<Scheda> schede)
        {
            CreateFileInFolder(path);
            using (_writer = XmlWriter.Create(path, new XmlWriterSettings() { Indent = true, Encoding = _encoding }))
            {
                WriteScheda(schede);
            }
        }
        String IPersister.FormatScheda(IEnumerable<Scheda> schede)
        {
            using (StringWriter stringWriter = new StringWriterWithEncoding(_encoding))
            {
                Encoding encoding = Encoding.UTF8;
                using (_writer = XmlTextWriter.Create(stringWriter, new XmlWriterSettings() { Indent = true, Encoding = _encoding }))
                {
                    WriteScheda(schede);
                }
                
                if (_writer.Settings.Encoding.BodyName != encoding.BodyName)
                    return stringWriter.ToString().Replace(stringWriter.Encoding.BodyName, encoding.BodyName);
                else
                    return stringWriter.ToString();
            }
        }
        private void WriteScheda(IEnumerable<Scheda> schede)
        {
            _writer.WriteStartDocument();
            _writer.WriteStartElement("Schede");
            foreach (Scheda scheda in schede)
            {
                _writer.WriteStartElement("Scheda");
                _writer.WriteAttributeString("name", scheda.IdScheda);
                foreach (String key in scheda.GetIds())
                {
                    _writer.WriteStartElement("Campo");
                    _writer.WriteAttributeString("name", key);
                    _writer.WriteAttributeString("valore", scheda.GetValore(key));
                    _writer.WriteEndElement();
                }
                _writer.WriteEndElement();
            }
            _writer.WriteEndElement();
            _writer.WriteEndDocument();
        }

        #endregion

        #region Descrittore
        
        IEnumerable<DescrittorePartita> IPersister.LoadDescrittorePartita(string path)
        {
            _xmlDocument.Load(path);
            return ReadDescrittorePartita();
        }
        IEnumerable<DescrittorePartita> IPersister.FormatDescrittorePartita(string Xml)
        {
            _xmlDocument.LoadXml(Xml);
            return ReadDescrittorePartita();
        }
        private IEnumerable<DescrittorePartita> ReadDescrittorePartita()
        {
            List<DescrittorePartita> descrittori = new List<DescrittorePartita>();
            XmlElement rootElement = (XmlElement)_xmlDocument.SelectSingleNode("Descrittori");
            if (rootElement == null) throw new IOException("Invalid Format Exception");

            foreach (XmlNode elemento in rootElement.ChildNodes)
            {
                if (elemento.LocalName != "Descrittore") throw new IOException("Invalid Format Exception - Descrittore non valido");
                DescrittorePartita s = new DescrittorePartita();
                if ((s.IdPartita = ((XmlElement)elemento).GetAttribute("idPartita")) == null) throw new IOException("Invalid Format Exception - Attributo non valido");
                if ((s.Nome = ((XmlElement)elemento).GetAttribute("nome")) == null) throw new IOException("Invalid Format Exception - Attributo non valido");
                if ((s.NomeMaster = ((XmlElement)elemento).GetAttribute("master")) == null) throw new IOException("Invalid Format Exception - Attributo non valido");
                if ((s.IpMaster = ((XmlElement)elemento).GetAttribute("ipMaster")) == null) throw new IOException("Invalid Format Exception - Attributo non valido");
                if ((s.Descrizione = ((XmlElement)elemento).GetAttribute("descrizione")) == null) throw new IOException("Invalid Format Exception - Attributo non valido");
                if ((s.Password = ((XmlElement)elemento).GetAttribute("password")) == null) throw new IOException("Invalid Format Exception - Attributo non valido");
                s.Stato = (StatoPartita)Int32.Parse(((XmlElement)elemento).GetAttribute("stato"));

                descrittori.Add(s);
            }
            return descrittori;
        }

        void IPersister.SaveDescrittorePartita(string path, IEnumerable<DescrittorePartita> descrittori)
        {
            CreateFileInFolder(path);
            using (_writer = XmlWriter.Create(path, new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 }))
            {
                WriteDescrittorePartita(descrittori);
            }
        }
        String IPersister.FormatDescrittorePartita(IEnumerable<DescrittorePartita> descrittori)
        {
            using (StringWriter stringWriter = new StringWriterWithEncoding(_encoding))
            {
                using (_writer = XmlWriter.Create(stringWriter, new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 }))
                {
                    WriteDescrittorePartita(descrittori);
                }
                return stringWriter.ToString();
            }
        }
        private void WriteDescrittorePartita(IEnumerable<DescrittorePartita> descrittori)
        {
            _writer.WriteStartDocument();
            _writer.WriteStartElement("Descrittori");
            foreach (DescrittorePartita descrittore in descrittori)
            {
                _writer.WriteStartElement("Descrittore");
                _writer.WriteAttributeString("idPartita", descrittore.IdPartita);
                _writer.WriteAttributeString("nome", descrittore.Nome);
                _writer.WriteAttributeString("master", descrittore.NomeMaster);
                _writer.WriteAttributeString("ipMaster", descrittore.IpMaster);
                _writer.WriteAttributeString("descrizione", descrittore.Descrizione);
                _writer.WriteAttributeString("password", descrittore.Password);
                _writer.WriteAttributeString("stato", ((int)descrittore.Stato).ToString());
                _writer.WriteEndElement();
            }
            _writer.WriteEndElement();
            _writer.WriteEndDocument();
        }

        #endregion
        
    }

    #region SupportClass
    class StringWriterWithEncoding : StringWriter 
    //Le stringhe adottano sempre di default UTF-16, per cambiare l'encoding devo dichiarare una sottoclasse che permetta di modificarlo, dato che ciò non è permesso da StringWriter.
    {
        private readonly Encoding _encoding;
        public StringWriterWithEncoding() {}
        public StringWriterWithEncoding(IFormatProvider formatProvider) : base(formatProvider) {}
        public StringWriterWithEncoding(StringBuilder sb) : base(sb) {}
        public StringWriterWithEncoding(StringBuilder sb, IFormatProvider formatProvider) : base(sb, formatProvider){}
        public StringWriterWithEncoding(Encoding encoding) {_encoding = encoding;}
        public StringWriterWithEncoding(IFormatProvider formatProvider, Encoding encoding): base(formatProvider) {_encoding = encoding;}
        public StringWriterWithEncoding(StringBuilder sb, Encoding encoding): base(sb) {_encoding = encoding; }
        public StringWriterWithEncoding(StringBuilder sb, IFormatProvider formatProvider, Encoding encoding): base(sb, formatProvider) {_encoding = encoding;}
        public override Encoding Encoding   {  get { return (null == _encoding) ? base.Encoding : _encoding; }
        }
    }
    #endregion
}
