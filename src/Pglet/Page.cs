using Newtonsoft.Json;
using Pglet.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public Control GetControl(string id)
        {
            return _index.ContainsKey(id) ? _index[id] : null;
        }

        internal Dictionary<string, Control> Index
        {
            get { return _index; }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _controls;
        }

        public string Title
        {
            get { return GetAttr("title"); }
            set { SetAttr("title", value); }
        }

        public bool VerticalFill
        {
            get { return GetBoolAttr("verticalFill"); }
            set { SetBoolAttr("verticalFill", value); }
        }

        public Align HorizontalAlign
        {
            get { return GetEnumAttr<Align>("horizontalAlign"); }
            set { SetEnumAttr("horizontalAlign", value); }
        }

        public Align VerticalAlign
        {
            get { return GetEnumAttr<Align>("verticalAlign"); }
            set { SetEnumAttr("verticalAlign", value); }
        }

        public int Gap
        {
            get { return GetIntAttr("gap"); }
            set { SetIntAttr("gap", value); }
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

        public string Hash
        {
            get { return GetAttr("hash"); }
            set { SetAttr("hash", value); }
        }

        public EventHandler OnClose
        {
            get { return GetEventHandler("close"); }
            set { SetEventHandler("close", value); }
        }

        public EventHandler OnHashChange
        {
            get { return GetEventHandler("hashChange"); }
            set { SetEventHandler("hashChange", value); }
        }

        protected override string ControlName => "page";

        public Page(Connection conn, string url)
        {
            Uid = Id = "page";
            _conn = conn;
            _conn.OnEvent = OnEvent;
            _url = url;
            _index[Id] = this;
        }

        internal async Task LoadHash()
        {
            Hash = await _conn.SendAsync("get page hash");
        }

        public void Add(params Control[] controls)
        {
            AddAsync(controls).GetAwaiter().GetResult();
        }

        public Task AddAsync(params Control[] controls)
        {
            _controls.AddRange(controls);
            return UpdateAsync();
        }

        public void Insert(int at, params Control[] controls)
        {
            InsertAsync(at, controls).GetAwaiter().GetResult();
        }

        public Task InsertAsync(int at, params Control[] controls)
        {
            _controls.InsertRange(at, controls);
            return UpdateAsync();
        }

        public new void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }

        public new Task UpdateAsync()
        {
            return UpdateAsync(this);
        }

        public void Update(params Control[] controls)
        {
            UpdateAsync(controls).GetAwaiter().GetResult();
        }

        public async Task UpdateAsync(params Control[] controls)
        {
            var addedControls = new List<Control>();
            var commands = new List<string>();

            foreach(var control in controls)
            {
                control.BuildUpdateCommands(_index, addedControls, commands);
            }

            if (commands.Count == 0)
            {
                return;
            }

            // execute commands
            var ids = await Connection.SendBatchAsync(commands);

            // update new controls
            int n = 0;
            foreach(var id in ids.Split('\n').SelectMany(l => l.Split(' ')).Where(id => !String.IsNullOrEmpty(id)))
            {
                addedControls[n].Uid = id;
                addedControls[n].Page = this;

                // add to index
                _index[id] = addedControls[n];

                n++;
            }
        }

        public Task RemoveAsync(params Control[] controls)
        {
            foreach(var control in controls)
            {
                _controls.Remove(control);
            }
            return UpdateAsync();
        }

        public void Remove(params Control[] controls)
        {
            RemoveAsync(controls).GetAwaiter().GetResult();
        }

        public Task RemoveAtAsync(int index)
        {
            _controls.RemoveAt(index);
            return UpdateAsync();
        }

        public void RemoveAt(int index)
        {
            RemoveAtAsync(index).GetAwaiter().GetResult();
        }

        public override async Task CleanAsync()
        {
            PreviousChildren.Clear();

            foreach (var child in GetChildren())
            {
                RemoveControlRecursively(_index, child);
            }

            await _conn.SendAsync($"clean {Uid}");
        }

        public async Task CloseAsync()
        {
            await _conn.SendAsync("close");
        }

        public void Error(string message)
        {
            ErrorAsync(message).GetAwaiter().GetResult();
        }

        public async Task ErrorAsync(string message)
        {
            await _conn.SendAsync($"error \"{message.Encode()}\"");
        }

        public void Close()
        {
            CloseAsync().GetAwaiter().GetResult();
        }

        private void OnEvent(Event e)
        {
            //Console.WriteLine($"Event: {e.Target} - {e.Name} - {e.Data}");

            // update control properties
            if (e.Target == "page" && e.Name == "change")
            {
                var allProps = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(e.Data);
                foreach (var props in allProps)
                {
                    var id = props["i"];
                    if (_index.ContainsKey(id))
                    {
                        foreach (var key in props.Keys.Where(k => k != "i"))
                        {
                            _index[id].SetAttr(key, props[key], dirty: false);
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
                    var control = _index[e.Target];

                    var ce = new ControlEvent
                    {
                        Target = e.Target,
                        Name = e.Name,
                        Data = e.Data,
                        Control = _index[e.Target],
                        Page = this
                    };
                    var t = Task.Run(() => controlHandlers[e.Name](ce));
                }
            }
        }
    }
}
