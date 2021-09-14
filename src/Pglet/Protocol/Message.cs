using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Protocol
{
    public class Message
    {
        public string Id { get; set; }
        public string Action { get; set; }
        public object Payload { get; set; }
    }
}
