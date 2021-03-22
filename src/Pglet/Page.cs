﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Text.Json;

namespace Pglet
{
    public class Page : Control
    {
        string _url;
        List<Control> _controls = new List<Control>();
        Dictionary<string, Control> _index = new Dictionary<string, Control>(StringComparer.OrdinalIgnoreCase);
        private Connection _conn;

        public Connection Connection
        {
            get { return _conn; }
        }

        public string Url
        {
            get { return _url; }
        }

        public List<Control> Controls
        {
            get { return _controls; }
        }

        protected override string ControlName => "page";

        public Control GetControl(string id)
        {
            return _index.ContainsKey(id) ? _index[id] : null;
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _controls;
        }

        public string ThemePrimaryColor
        {
            get { return GetAttr("themePrimaryColor"); }
            set { SetAttr("themePrimaryColor", value); }
        }

        public string ThemeTextColor
        {
            get { return GetAttr("themeTextColor"); }
            set { SetAttr("themeTextColor", value); }
        }

        public string ThemeBackgroundColor
        {
            get { return GetAttr("themeBackgroundColor"); }
            set { SetAttr("themeBackgroundColor", value); }
        }

        public Page(Connection conn, string url) : base(id: "page")
        {
            _conn = conn;
            _conn.OnEvent = OnEvent;
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
            var addedControls = new List<Control>();
            var commands = new List<string>();

            foreach(var control in controls)
            {
                control.BuildUpdateCommands(_index, addedControls, commands);
            }

            // execute commands
            var ids = await Connection.SendBatchAsync(commands);

            // update new controls
            int n = 0;
            foreach(var id in ids.Split('\n').SelectMany(l => l.Split(' ')).Where(id => !String.IsNullOrEmpty(id)))
            {
                addedControls[n].Id = id;
                addedControls[n].Page = this;

                // add to index
                _index[id] = addedControls[n];

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

        private void OnEvent(Event e)
        {
            //Console.WriteLine($"Event: {e.Target} - {e.Name} - {e.Data}");

            // update control properties
            if (e.Target == this.Id && e.Name == "change")
            {
                var allProps = JsonSerializer.Deserialize<Dictionary<string, string>[]>(e.Data);
                foreach(var props in allProps)
                {
                    var id = props["i"];
                    if (_index.ContainsKey(id))
                    {
                        foreach(var key in props.Keys.Where(k => k != "i"))
                        {
                            _index[id].SetAttr(key, props[key]);
                        }
                    }
                }
            }
            // call event handlers
            else if (_index.ContainsKey(e.Target))
            {
                var controlHandlers = _index[e.Target].EventHandlers;
                if (controlHandlers.ContainsKey(e.Name))
                {
                    var t = Task.Run(() => controlHandlers[e.Name](e));
                }
            }
        }
    }
}
