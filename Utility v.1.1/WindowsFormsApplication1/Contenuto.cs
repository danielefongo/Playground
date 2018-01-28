using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public enum TipoValore {Numero,Testo}

    public class Contenuto : ElementoTemplate
    {
        private TipoValore _tipo;
        private String _formulaProva;
        private Boolean _modificabile;
        private Boolean _confermaMaster;
        private Boolean _identificatorePersonaggio;

        public Contenuto(String id, TipoValore tipo, String formulaProva, Boolean modificabile, Boolean confermaMaster, Boolean identificatorePersonaggio)
        {
            _id = id;
            _tipo = tipo;
            _formulaProva = formulaProva;
            _modificabile = modificabile;
            _confermaMaster = confermaMaster;
            _identificatorePersonaggio = identificatorePersonaggio;
        }
        public Contenuto(String id, TipoValore tipo, String formulaProva) : this(id,tipo, formulaProva, true, true, false) { }

        #region Properties
        public TipoValore Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        public String FormulaProva
        {
            get { return _formulaProva; }
            set { _formulaProva = value; }
        }

        public Boolean Modificabile
        {
            get { return _modificabile; }
            set { _modificabile = value; }
        }

        public Boolean ConfermaMaster
        {
            get { return _confermaMaster; }
            set { _confermaMaster = value; }
        }

        public Boolean IdentificatorePersonaggio
        {
            get { return _identificatorePersonaggio; }
            set { _identificatorePersonaggio = value; }
        }
        #endregion
    }
}
