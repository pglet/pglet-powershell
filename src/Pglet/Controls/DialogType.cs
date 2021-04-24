using System.ComponentModel;

namespace Pglet.Controls
{
    public enum DialogType
    {
        [Description("normal")]
        Normal,

        [Description("largeHeader")]
        LargeHeader,

        [Description("close")]
        Close
    }
}
