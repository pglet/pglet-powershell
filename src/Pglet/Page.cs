using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Pglet
{
    public class Page : Control
    {
        string _url;
        Connection _conn;
        List<Control> _controls = new List<Control>();

        public string Url
        {
            get { return _url; }
        }

        public Connection Connection
        {
            get { return _conn;  }
        }

        public List<Control> Controls
        {
            get { return _controls; }
        }

        protected override string ControlName => "page";

        protected override IEnumerable<Control> GetChildren()
        {
            return _controls;
        }

        public Page(Connection conn, string url) : base(id: "page")
        {
            _conn = conn;
            _url = url;
        }

        public Task Add(params Control[] controls)
        {
            _controls.AddRange(controls);
            return Update();
        }

        public Task Add(int at, params Control[] controls)
        {
            return Task.CompletedTask;
        }

        public Task Update()
        {
            return Update(this);
        }

        public async Task Update(params Control[] controls)
        {
            var index = new List<Control>();
            var commands = new List<string>();

            foreach(var control in controls)
            {
                control.BuildUpdateCommands(index, commands);
            }

            // execute commands
            var ids = await _conn.SendBatchAsync(commands);
            int n = 0;
            foreach(var id in ids.Split('\n').SelectMany(l => l.Split(' ')))
            {
                index[n].Id = id;
                n++;
            }
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
