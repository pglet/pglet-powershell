using System.ComponentModel;

namespace Pglet.Controls
{
    public enum BorderStyle
    {
        [Description("dotted")]
        Dotted,

        [Description("dashed")]
        Dashed,

        [Description("solid")]
        Solid,

        [Description("double")]
        Double,

        [Description("groove")]
        Groove,

        [Description("ridge")]
        Ridge,

        [Description("inset")]
        Inset,

        [Description("outset")]
        Outset
    }
}
