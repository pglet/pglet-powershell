using System.Text.Json;

namespace Pglet
{
    public class ControlEvent : Event
    {
        public Control Control { get; set; }
        public Page Page { get; set; }
    }
}
