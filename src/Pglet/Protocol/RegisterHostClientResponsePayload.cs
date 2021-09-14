using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Protocol
{
    public class RegisterHostClientResponsePayload
    {
        public string HostClientID { get; set; }
        public string SessionID { get; set; }
        public string PageName { get; set; }
        public string Error { get; set; }
    }

}
