using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Protocol
{
    public class Command
    {
        public int Indent { get; set; }
        public string Name { get; set; }
        public List<string> Values { get; set; }
        public Dictionary<string, string> Attrs { get; set; }
        public List<string> Lines { get; set; }
    }
}
