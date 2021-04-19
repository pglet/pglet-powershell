using System.ComponentModel;

namespace Pglet.Controls
{
    public enum PanelType
    {
        [Description("small")]
        Small,

        [Description("smallLeft")]
        SmallLeft,

        [Description("medium")]
        Medium,

        [Description("large")]
        Large,

        [Description("largeFixed")]
        LargeFixed,

        [Description("extraLarge")]
        ExtraLarge,

        [Description("fluid")]
        Fluid,

        [Description("custom")]
        Custom,

        [Description("customLeft")]
        CustomLeft
    }
}
