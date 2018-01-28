using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    abstract public class ElementoTemplate
    {
        protected String _id;
        protected Contenitore _parent;

        public Contenitore Parent
        {
            get { return _parent; }
            set { 
                
                if (value != _parent)
                {
                    if (_parent != null) _parent.Children.Remove(this);
                    if (value != null) value.Children.Add(this);
                }
            }
        }

        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

    }

}
