using System.ComponentModel;

namespace Pglet.Controls
{
    public enum Align
    {
        [Description("start")]
        Start,

        [Description("end")]
        End,

        [Description("center")]
        Center,

        [Description("space-between")]
        SpaceBetween,

        [Description("space-around")]
        SpaceAround,

        [Description("space-evenly")]
        SpaceEvenly,

        [Description("baseline")]
        Baseline,

        [Description("stretch")]
        Stretch
    }
}
