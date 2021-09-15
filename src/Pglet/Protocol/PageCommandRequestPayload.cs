using System.Collections.Generic;

namespace Pglet.Protocol
{
    public class PageCommandRequestPayload
    {
        public string PageName { get; set; }
        public string SessionID { get; set; }
        public Command Command { get; set; }
    }
}
