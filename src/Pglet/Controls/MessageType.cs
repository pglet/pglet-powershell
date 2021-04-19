using System.ComponentModel;

namespace Pglet.Controls
{
    public enum MessageType
    {
        [Description("info")]
        Info,

        [Description("error")]
        Error,

        [Description("blocked")]
        Blocked,

        [Description("severeWarning")]
        SevereWarning,

        [Description("success")]
        Success,

        [Description("warning")]
        Warning
    }
}
