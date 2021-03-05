using System.Text.Json;

namespace Pglet
{
    public class Event
    {
        public string Target { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
