using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public class Contenitore : ElementoTemplate
    {
        private List<ElementoTemplate> _children;

        public Contenitore(String id) {
            _parent = null;
            _id = id;
            _children = new List<ElementoTemplate>();
        }

        public List<ElementoTemplate> Children
        {
            get { return _children; }
        }

    }
}
