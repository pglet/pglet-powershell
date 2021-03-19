using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pglet
{
    public class Page : Control
    {
        string _url;
        Connection _conn;
        IList<Control> _controls = new List<Control>();

        public string Url
        {
            get { return _url; }
        }

        public Connection Connection
        {
            get { return _conn;  }
        }

        public IList<Control> Controls
        {
            get { return _controls; }
        }

        protected override string ControlName => "page";

        public Page(Connection conn, string url) : base(id: "page")
        {
            _conn = conn;
            _url = url;
        }

        public Task Add(params Control[] controls)
        {
            return Task.CompletedTask;
        }

        public Task Add(int at, params Control[] controls)
        {
            return Task.CompletedTask;
        }

        public Task Update()
        {
            return Task.CompletedTask;
        }

        public Task Update(params Control[] controls)
        {
            return Task.CompletedTask;
        }

        public Task Remove(params Control[] controls)
        {
            return Task.CompletedTask;
        }

        public Task Remove(int at)
        {
            return Task.CompletedTask;
        }

        public Task Clean()
        {
            return Task.CompletedTask;
        }

        public Task Clean(int at)
        {
            return Task.CompletedTask;
        }
    }
}
