using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pglet
{
    public class Page
    {
        string _url;
        Connection _conn;
        IList<Control> _controls = new List<Control>();

        public Connection Connection
        {
            get { return _conn;  }
        }

        public IList<Control> Controls
        {
            get { return _controls; }
        }

        public Page(Connection conn, string url)
        {
            _conn = conn;
            _url = url;
        }

        /// <summary>
        /// Performs one-way synchronization of page controls to Pglet Server.
        /// </summary>
        /// <returns></returns>
        public Task Update()
        {
            return Task.CompletedTask;
        }
    }
}
