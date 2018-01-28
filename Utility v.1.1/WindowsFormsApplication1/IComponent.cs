using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Messages;

namespace Utility
{
    public interface IComponent
    {
        void Input(MessageEventArgs messaggio);
        event Evento Output;
    }

    public interface IVisualizable : IComponent
    {
        int DisplayHeight {get; set;}
        int DisplayWidth { get; set; }
        DimensionType HeightType { get; set; }
        DimensionType WidthType { get; set; }
    }

    public enum DimensionType { Pixel, Percent }
}
