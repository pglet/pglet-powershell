using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Protocol
{
    public class PageCommandsBatchRequestPayload
    {
        public string PageName { get; set; }
        public string SessionID { get; set; }
        public List<Command> Commands { get; set; }
    }
}
