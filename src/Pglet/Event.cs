using System;
using System.Collections.Generic;
using System.Text;
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
