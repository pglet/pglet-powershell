using Newtonsoft.Json;
using Pglet.Controls;
using Pglet.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet
{
    public class Page : Control, IDisposable
    {
        readonly Connection _conn;
        readonly string _pageUrl;
        readonly string _pageName;
        readonly string _sessionId;
        readonly List<Control> _controls = new List<Control>();
        readonly Dictionary<string, Control> _index = new Dictionary<string, Control>(StringComparer.OrdinalIgnoreCase);
        readonly ConcurrentDictionary<TaskCompletionSource<ControlEvent>, CancellationTokenRegistration> _pageEventListeners = new();

        public Connection Connection
        {
            get { return _conn; }
        }

        public string SessionId
        {
            get { return _sessionId; }
        }

        public IList<Control> Controls
        {
            get
            {
                return _controls;
            }
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

        public string Url
        {
            get { return _pageUrl; }
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

        public string BgColor
        {
            get { return GetAttr("bgcolor"); }
            set { SetAttr("bgcolor", value); }
        }        

        public string Theme
        {
            get { return GetAttr("theme"); }
            set { SetAttr("theme", value); }
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

        public int WinWidth
        {
            get { return GetIntAttr("win_width"); }
        }

        public int WinHeight
        {
            get { return GetIntAttr("win_height"); }
        }

        public string Signin
        {
            get { return GetAttr("signin"); }
            set { SetAttr("signin", value); }
        }

        public bool SigninAllowDismiss
        {
            get { return GetBoolAttr("signinAllowDismiss"); }
            set { SetBoolAttr("signinAllowDismiss", value); }
        }

        public bool SigninGroups
        {
            get { return GetBoolAttr("signinGroups"); }
            set { SetBoolAttr("signinGroups", value); }
        }

        public string UserId
        {
            get { return GetAttr("userId"); }
            set { SetAttr("userId", value); }
        }

        public string UserLogin
        {
            get { return GetAttr("userLogin"); }
            set { SetAttr("userLogin", value); }
        }

        public string UserName
        {
            get { return GetAttr("userName"); }
            set { SetAttr("userName", value); }
        }

        public string UserEmail
        {
            get { return GetAttr("userEmail"); }
            set { SetAttr("userEmail", value); }
        }

        public string UserClientIP
        {
            get { return GetAttr("userClientIP"); }
            set { SetAttr("userClientIP", value); }
        }

        public EventHandler OnClose
        {
            get { return GetEventHandler("close"); }
            set { SetEventHandler("close", value); }
        }

        public EventHandler OnSignin
        {
            get { return GetEventHandler("signin"); }
            set { SetEventHandler("signin", value); }
        }

        public EventHandler OnDismissSignin
        {
            get { return GetEventHandler("dismissSignin"); }
            set { SetEventHandler("dismissSignin", value); }
        }

        public EventHandler OnSignout
        {
            get { return GetEventHandler("signout"); }
            set { SetEventHandler("signout", value); }
        }

        public EventHandler OnHashChange
        {
            get { return GetEventHandler("hashChange"); }
            set { SetEventHandler("hashChange", value); }
        }

        public EventHandler OnResize
        {
            get { return GetEventHandler("resize"); }
            set { SetEventHandler("resize", value); }
        }

        protected override string ControlName => "page";

        public Page(Connection conn, string pageUrl, string pageName, string sessionId) : base()
        {
            UniqueId = _id = "page";
            _conn = conn;
            _pageUrl = pageUrl;
            _pageName = pageName;
            _sessionId = sessionId;
            _index[UniqueId] = this;
        }

        internal async Task LoadPageDetails()
        {
            var values = (await _conn.SendCommands(_pageName, _sessionId, new List<Command>
            {
                new Protocol.Command { Name = "get", Values = new List<string> { "page", "hash" } },
                new Protocol.Command { Name = "get", Values = new List<string> { "page", "win_width" } },
                new Protocol.Command { Name = "get", Values = new List<string> { "page", "win_height" } },
                new Protocol.Command { Name = "get", Values = new List<string> { "page", "userid" } },
                new Protocol.Command { Name = "get", Values = new List<string> { "page", "userlogin" } },
                new Protocol.Command { Name = "get", Values = new List<string> { "page", "username" } },
                new Protocol.Command { Name = "get", Values = new List<string> { "page", "useremail" } },
                new Protocol.Command { Name = "get", Values = new List<string> { "page", "userclientip" } }
            }, CancellationToken.None)).Results;

            SetAttr("hash", values[0], dirty: false);
            SetAttr("win_width", values[1], dirty: false);
            SetAttr("win_height", values[2], dirty: false);
            SetAttr("userid", values[3], dirty: false);
            SetAttr("userlogin", values[4], dirty: false);
            SetAttr("username", values[5], dirty: false);
            SetAttr("useremail", values[6], dirty: false);
            SetAttr("userclientip", values[7], dirty: false);
        }

        public void Add(params Control[] controls)
        {
            AddAsync(controls).GetAwaiter().GetResult();
        }

        public void Add(CancellationToken cancellationToken, params Control[] controls)
        {
            AddAsync(cancellationToken, controls).GetAwaiter().GetResult();
        }

        public async Task AddAsync(params Control[] controls)
        {
            using (var cts = new CancellationTokenSource(DEFAULT_COMMAND_TIMEOUT_MS))
            {
                await AddAsync(cts.Token, controls);
            }
        }

        public async Task AddAsync(CancellationToken cancellationToken, params Control[] controls)
        {
            await _lock.WaitAsync(cancellationToken);
            try
            {
                _controls.AddRange(controls);
                await UpdateAsync(cancellationToken, this);
            }
            finally
            {
                _lock.Release();
            }
        }
        
        public void Insert(int at, params Control[] controls)
        {
            InsertAsync(at, controls).GetAwaiter().GetResult();
        }

        public void Insert(CancellationToken cancellationToken, int at, params Control[] controls)
        {
            InsertAsync(cancellationToken, at, controls).GetAwaiter().GetResult();
        }

        public async Task InsertAsync(int at, params Control[] controls)
        {
            using (var cts = new CancellationTokenSource(DEFAULT_COMMAND_TIMEOUT_MS))
            {
                await InsertAsync(cts.Token, at, controls);
            }
        }

        public async Task InsertAsync(CancellationToken cancellationToken, int at, params Control[] controls)
        {
            await _lock.WaitAsync(cancellationToken);
            try
            {
                _controls.InsertRange(at, controls);
                await UpdateAsync(cancellationToken, this);
            }
            finally
            {
                _lock.Release();
            }            
        }

        public new void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }        

        public new void Update(CancellationToken cancellationToken)
        {
            UpdateAsync(cancellationToken).GetAwaiter().GetResult();
        }

        public new async Task UpdateAsync()
        {
            using (var cts = new CancellationTokenSource(DEFAULT_COMMAND_TIMEOUT_MS))
            {
                await UpdateAsync(cts.Token);
            }
        }        

        public new async Task UpdateAsync(CancellationToken cancellationToken)
        {
            await _lock.WaitAsync(cancellationToken);
            try
            {
                await UpdateAsync(cancellationToken, this);
            }
            finally
            {
                _lock.Release();
            }             
        }

        public async Task UpdateAsync(CancellationToken cancellationToken, params Control[] controls)
        {
            var addedControls = new List<Control>();
            var commands = new List<Command>();

            foreach (var control in controls)
            {
                control.BuildUpdateCommands(_index, addedControls, commands);
            }

            if (commands.Count == 0)
            {
                return;
            }

            // execute commands
            var ids = (await _conn.SendCommands(_pageName, _sessionId, commands, cancellationToken)).Results;

            // update new controls
            int n = 0;
            foreach (var id in ids.SelectMany(l => l.Split(' ')).Where(id => !String.IsNullOrEmpty(id)))
            {
                addedControls[n].UniqueId = id;
                addedControls[n].Page = this;

                // add to index
                _index[id] = addedControls[n];

                n++;
            }
        }

        public ControlEvent WaitEvent(CancellationToken cancellationToken)
        {
            return WaitEventAsync(cancellationToken).GetAwaiter().GetResult();
        }

        public Task<ControlEvent> WaitEventAsync(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<ControlEvent>();

            tcs.Task.ContinueWith(t =>
            {
                if(_pageEventListeners.TryRemove(tcs, out CancellationTokenRegistration ctr))
                {
                    ctr.Dispose();
                }
            });

            var ctr = cancellationToken.Register(() =>
            {
                tcs.SetCanceled();
            });

            _pageEventListeners.TryAdd(tcs, ctr);

            return tcs.Task;
        }

        public bool ShowSignin(string authProviders, bool withGroups, bool allowDismiss, CancellationToken cancellationToken)
        {
            return ShowSigninAsync(authProviders, withGroups, allowDismiss, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<bool> ShowSigninAsync(string authProviders, bool withGroups, bool allowDismiss, CancellationToken cancellationToken)
        {
            await _lock.WaitAsync(cancellationToken);
            try
            {
                this.Signin = authProviders;
                this.SigninGroups = withGroups;
                this.SigninAllowDismiss = allowDismiss;
                await UpdateAsync(cancellationToken, this);
            }
            finally
            {
                _lock.Release();
            }

            // wait for events
            while (!cancellationToken.IsCancellationRequested)
            {
                var e = await WaitEventAsync(cancellationToken);
                if (e.Control == this && e.Name.Equals("signin", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else if (e.Control == this && e.Name.Equals("dismissSignin", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return false;
        }

        public void Signout()
        {
            SignoutAsync().GetAwaiter().GetResult();
        }

        public void Signout(CancellationToken cancellationToken)
        {
            SignoutAsync(cancellationToken).GetAwaiter().GetResult();
        }        

        public async Task SignoutAsync()
        {
            using (var cts = new CancellationTokenSource(DEFAULT_COMMAND_TIMEOUT_MS))
            {
                await SignoutAsync(cts.Token);
            }
        }        

        public async Task SignoutAsync(CancellationToken cancellationToken)
        {
            await SendCommand(cancellationToken, "signout");
        }

        public bool CanAccess(string usersAndGroups)
        {
            return CanAccessAsync(usersAndGroups).GetAwaiter().GetResult();
        }        

        public bool CanAccess(CancellationToken cancellationToken, string usersAndGroups)
        {
            return CanAccessAsync(cancellationToken, usersAndGroups).GetAwaiter().GetResult();
        }

        public async Task<bool> CanAccessAsync(string permissions)
        {
            using (var cts = new CancellationTokenSource(DEFAULT_COMMAND_TIMEOUT_MS))
            {
                return await CanAccessAsync(cts.Token, permissions);
            }
        }

        public async Task<bool> CanAccessAsync(CancellationToken cancellationToken, string permissions)
        {
            return (await SendCommand(cancellationToken, "canAccess", permissions)).Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        public void Remove(params Control[] controls)
        {
            RemoveAsync(controls).GetAwaiter().GetResult();
        }        

        public void Remove(CancellationToken cancellationToken, params Control[] controls)
        {
            RemoveAsync(cancellationToken, controls).GetAwaiter().GetResult();
        }

        public async Task RemoveAsync(params Control[] controls)
        {
            using (var cts = new CancellationTokenSource(DEFAULT_COMMAND_TIMEOUT_MS))
            {
                await RemoveAsync(cts.Token, controls);
            }
        }

        public async Task RemoveAsync(CancellationToken cancellationToken, params Control[] controls)
        {
            await _lock.WaitAsync(cancellationToken);
            try
            {
                foreach (var control in controls)
                {
                    _controls.Remove(control);
                }
                await UpdateAsync(cancellationToken, this);
            }
            finally
            {
                _lock.Release();
            }
        }

        public void RemoveAt(int index)
        {
            RemoveAtAsync(index).GetAwaiter().GetResult();
        }        

        public void RemoveAt(CancellationToken cancellationToken, int index)
        {
            RemoveAtAsync(cancellationToken, index).GetAwaiter().GetResult();
        }        

        public async Task RemoveAtAsync(int index)
        {
            using (var cts = new CancellationTokenSource(DEFAULT_COMMAND_TIMEOUT_MS))
            {
                await RemoveAtAsync(cts.Token, index);
            }
        }

        public async Task RemoveAtAsync(CancellationToken cancellationToken, int index)
        {
            await _lock.WaitAsync(cancellationToken);
            try
            {
                _controls.RemoveAt(index);
                await UpdateAsync(cancellationToken, this);
            }
            finally
            {
                _lock.Release();
            }
        }

        public override async Task CleanAsync(CancellationToken cancellationToken)
        {
            await _lock.WaitAsync(cancellationToken);
            try
            {
                _previousChildren.Clear();

                foreach (var child in GetChildren())
                {
                    RemoveControlRecursively(_index, child);
                }

                _controls.Clear();

                await SendCommand(cancellationToken, "clean", UniqueId);
            }
            finally
            {
                _lock.Release();
            }
        }

        public void Error(string message)
        {
            ErrorAsync(message).GetAwaiter().GetResult();
        }        

        public void Error(CancellationToken cancellationToken, string message)
        {
            ErrorAsync(cancellationToken, message).GetAwaiter().GetResult();
        }

        public async Task ErrorAsync(string message)
        {
            using (var cts = new CancellationTokenSource(DEFAULT_COMMAND_TIMEOUT_MS))
            {
                await ErrorAsync(cts.Token, message);
            }
        }

        public async Task ErrorAsync(CancellationToken cancellationToken, string message)
        {
            await SendCommand(cancellationToken, "error", message);
        }

        public void Close()
        {
            if (_sessionId == PgletClient.ZERO_SESSION)
            {
                _conn.Close();
            }
        }

        public async Task OnEvent(Event e)
        {
            //Console.WriteLine($"Event: {e.Target} - {e.Name} - {e.Data}");

            // update control properties
            if (e.Target == "page" && e.Name == "change")
            {
                await _lock.WaitAsync();
                try
                {
                    var allProps = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(e.Data);
                    foreach (var props in allProps)
                    {
                        var id = props["i"];
                        if (_index.ContainsKey(id))
                        {
                            foreach (var key in props.Keys.Where(k => k != "i"))
                            {
                                _index[id].SetAttrInternal(key, props[key], dirty: false);
                            }
                        }
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }
            // call event handlers
            else if (_index.ContainsKey(e.Target))
            {
                var ce = new ControlEvent
                {
                    Target = e.Target,
                    Name = e.Name,
                    Data = e.Data,
                    Control = _index[e.Target],
                    Page = this
                };

                var controlHandlers = _index[e.Target].EventHandlers;
                if (controlHandlers.ContainsKey(e.Name))
                {
                    var control = _index[e.Target];
                    var t = Task.Run(() => controlHandlers[e.Name](ce));
                }

                // notify event listeners
                foreach(var tcs in _pageEventListeners.Keys.ToArray())
                {
                    tcs.TrySetResult(ce);
                }
            }
        }

        public async Task<string> SendCommand(CancellationToken cancellationToken, string name, params string[] values)
        {
            return (await _conn.SendCommand(_pageName, _sessionId, new Protocol.Command { Name = name, Values = values.ToList() }, cancellationToken)).Result;
        }

        public void Dispose()
        {
            Close();
        }
    }
}
