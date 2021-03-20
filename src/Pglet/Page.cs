using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Pglet
{
    public class Page : Control
    {
        string _url;
        List<Control> _controls = new List<Control>();

        public string Url
        {
            get { return _url; }
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
            Connection = conn;
            _url = url;
        }

        public Task Add(params Control[] controls)
        {
            _controls.AddRange(controls);
            return Update();
        }

        public Task Insert(int at, params Control[] controls)
        {
            _controls.InsertRange(at, controls);
            return Update();
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
            var ids = await Connection.SendBatchAsync(commands);

            // update new controls
            int n = 0;
            foreach(var id in ids.Split('\n').SelectMany(l => l.Split(' ')).Where(id => !String.IsNullOrEmpty(id)))
            {
                index[n].Id = id;
                index[n].Connection = Connection;

                // re-subscribe event handlers
                foreach(var evt in index[n].EventHandlers)
                {
                    Connection.AddEventHandler(id, evt.Key, evt.Value);
                }

                n++;
            }
        }

        public Task Remove(params Control[] controls)
        {
            foreach(var control in controls)
            {
                _controls.Remove(control);
            }
            return Update();
        }

        public Task RemoveAt(int index)
        {
            _controls.RemoveAt(index);
            return Update();
        }

        public Task Clean()
        {
            _controls.Clear();
            return Update();
        }
    }
}
