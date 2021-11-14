using System.Collections.Generic;

namespace Pglet.Protocol
{
    public class PageCommandsBatchResponsePayload
    {
        public List<string> Results { get; set; }
        public string Error { get; set; }
    }
}
