using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Protocol
{
    public class PageCommandsBatchResponsePayload
    {
        public List<string> Results { get; set; }
        public string Error { get; set; }
    }
}
