using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Protocol
{
    public class RegisterHostClientPayload
    {
        public string HostClientID { get; set; }
        public string PageName { get; set; }
        public bool IsApp { get; set; }
        public string AuthToken { get; set; }
        public string Permissions { get; set; }
    }

}
