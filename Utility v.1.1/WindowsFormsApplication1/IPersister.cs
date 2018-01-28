using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public interface IPersister
    {
        ElementoTemplate LoadTemplate(string path);
        ElementoTemplate FormatTemplate(string Xml);
        void SaveTemplate(string path, ElementoTemplate root);
        String FormatTemplate(ElementoTemplate root);
        IEnumerable<Scheda> LoadScheda(string path);
        IEnumerable<Scheda> FormatScheda(string Xml);
        void SaveScheda(string path, IEnumerable<Scheda> schede);
        String FormatScheda(IEnumerable<Scheda> schede);
        IEnumerable<DescrittorePartita> LoadDescrittorePartita(string path);
        IEnumerable<DescrittorePartita> FormatDescrittorePartita(string Xml);
        void SaveDescrittorePartita(string path, IEnumerable<DescrittorePartita> descrittori);
        String FormatDescrittorePartita(IEnumerable<DescrittorePartita> descrittori);
    }
}
