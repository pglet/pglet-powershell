using System.Collections.Generic;

namespace Pglet.Protocol
{
    public class PageCommandsBatchRequestPayload
    {
        public string PageName { get; set; }
        public string SessionID { get; set; }
        public List<Command> Commands { get; set; }
    }
}
