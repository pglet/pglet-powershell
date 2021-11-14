using System.Collections.Generic;

namespace Pglet.Protocol
{
    public class Command
    {
        public int Indent { get; set; }
        public string Name { get; set; }
        public List<string> Values { get; set; } = new List<string>();
        public Dictionary<string, string> Attrs { get; set; } = new Dictionary<string, string>();
        public List<string> Lines { get; set; } = new List<string>();
        public List<Command> Commands { get; set; } = new List<Command>();
    }
}
