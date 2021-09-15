using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Protocol
{
    public class PageEventPayload
    {
        public string PageName { get; set; }
        public string SessionID { get; set; }
        public string EventTarget { get; set; }
        public string EventName { get; set; }
        public string EventData { get; set; }
    }
}
