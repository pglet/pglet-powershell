using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Pglet
{
    public abstract class Control
    {
        public class AttrValue
        {
            public string Value { get; set; }
            public bool IsDirty { get; set; }
        }

        protected abstract string ControlName { get; }

        private Dictionary<string, AttrValue> _attrs = new Dictionary<string, AttrValue>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, EventHandler> _eventHandlers = new Dictionary<string, EventHandler>(StringComparer.OrdinalIgnoreCase);
        private List<Control> _previousChildren = new List<Control>(); // hash codes of previous children
        private string _uid;
        private Page _page;
        private object _data;

        public Page Page
        {
            get { return _page; }
            internal set { _page = value; }
        }

        public string Uid
        {
            get { return _uid; }
            internal set { _uid = value; }
        }

        public string Id
        {
            get { return GetAttr("id"); }
            set { SetAttr("id", value); }
        }

        public string Width
        {
            get { return GetAttr("width"); }
            set { SetAttr("width", value); }
        }

        public string Height
        {
            get { return GetAttr("height"); }
            set { SetAttr("height", value); }
        }

        public string Padding
        {
            get { return GetAttr("padding"); }
            set { SetAttr("padding", value); }
        }

        public string Margin
        {
            get { return GetAttr("margin"); }
            set { SetAttr("margin", value); }
        }

        public bool Visible
        {
            get { return GetBoolAttr("visible", true); }
            set { SetBoolAttr("visible", value); }
        }

        public bool Disabled
        {
            get { return GetBoolAttr("disabled", false); }
            set { SetBoolAttr("disabled", value); }
        }

        public object Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                if (value != null)
                {
                    SetAttr("data", value.ToString());
                }
            }
        }

        public void Update()
        {
            if (_page != null)
            {
                _page.Update(this);
            }
        }

        public async Task UpdateAsync()
        {
            if (_page != null)
            {
                await _page.UpdateAsync(this);
            }
        }

        internal Dictionary<string, EventHandler> EventHandlers
        {
            get { return _eventHandlers; }
        }

        protected virtual IEnumerable<Control> GetChildren()
        {
            return new Control[] {};
        }

        internal List<Control> PreviousChildren
        {
            get { return _previousChildren; }
        }

        protected void SetEventHandler(string eventName, EventHandler handler)
        {
            if (handler != null)
            {
                _eventHandlers[eventName] = handler;
            }
        }

        protected EventHandler GetEventHandler(string eventName)
        {
            return _eventHandlers.ContainsKey(eventName) ? _eventHandlers[eventName] : null;
        }

        protected void SetEnumAttr<T>(string name, T value, bool dirty = true)
        {
            var strValue = value.ToString();
            var memberInfo = typeof(T).GetMember(strValue);
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    strValue = ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            SetAttr(name, strValue, dirty);
        }

        protected T GetEnumAttr<T>(string name) where T : struct
        {
            if (_attrs.ContainsKey(name))
            {
                if (Enum.TryParse(_attrs[name].Value, out T result))
                {
                    return result;
                }
                else
                {
                    var strValue = _attrs[name].Value;
                    foreach (var field in typeof(T).GetFields())
                    {
                        if (Attribute.GetCustomAttribute(field,
                        typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                        {
                            if (attribute.Description.Equals(strValue, StringComparison.OrdinalIgnoreCase))
                                return (T)field.GetValue(null);
                        }
                        else
                        {
                            if (field.Name.Equals(strValue, StringComparison.OrdinalIgnoreCase))
                                return (T)field.GetValue(null);
                        }
                    }
                }
            }
            return default;
        }

        protected void SetIntAttr(string name, int value)
        {
            SetAttr(name, value.ToString().ToLowerInvariant());
        }

        internal int GetIntAttr(string name, int defValue = 0)
        {
            return _attrs.ContainsKey(name) ? Int32.Parse(_attrs[name].Value) : defValue;
        }

        protected void SetBoolAttr(string name, bool value)
        {
            SetAttr(name, value.ToString().ToLowerInvariant());
        }

        internal bool GetBoolAttr(string name, bool defValue = false)
        {
            return _attrs.ContainsKey(name) ? Boolean.Parse(_attrs[name].Value) : defValue;
        }

        internal void SetAttr(string name, string value, bool dirty = true)
        {
            if (value != null)
            {
                _attrs[name] = new AttrValue { Value = value, IsDirty = dirty };
            }
        }

        protected string GetAttr(string name)
        {
            return _attrs.ContainsKey(name) ? _attrs[name].Value : null;
        }

        internal void SetAttr<T>(string name, T value, bool dirty = true) where T : notnull
        {
            if (value != null)
            {
                _attrs[name] = new AttrValue { Value = value.ToString(), IsDirty = dirty };
            }
        }

        protected T GetAttr<T>(string name) where T :  notnull
        {
            var sval = _attrs.ContainsKey(name) ? _attrs[name].Value : null;
            if (sval == null)
            {
                return default(T);
            }
            else if (typeof(T) == typeof(int))
            {
                return int.TryParse(sval, out int result) ? (T)(object)result: (T)(object)0;
            }
            else if (typeof(T) == typeof(long))
            {
                return long.TryParse(sval, out long result) ? (T)(object)result : (T)(object)0;
            }
            else if (typeof(T) == typeof(float))
            {
                return float.TryParse(sval, out float result) ? (T)(object)result : (T)(object)0;
            }
            else
            {
                return (T)(object)sval;
            }
        }

        internal void BuildUpdateCommands(Dictionary<string, Control> index, List<Control> addedControls, List<string> commands)
        {
            // update control settings
            var updateAttrs = this.GetCommandAttrs(update: true);

            if (updateAttrs.Count > 0)
            {
                commands.Add($"set {string.Join(" ", updateAttrs)}");
            }

            // go through children
            var previousChildren = this.PreviousChildren;
            var currentChildren = this.GetChildren();

            var previousHashes = new Dictionary<int, Control>();
            var currentHashes = new Dictionary<int, Control>();

            foreach (var ctrl in previousChildren)
            {
                previousHashes[ctrl.GetHashCode()] = ctrl;
            }
            foreach (var ctrl in currentChildren)
            {
                currentHashes[ctrl.GetHashCode()] = ctrl;
            }

            var previousInts = previousHashes.Keys.ToArray();
            var currentInts = currentHashes.Keys.ToArray();

            // diff sequences
            var diffs = Diff.DiffInt(previousInts, currentInts);

            int n = 0;
            for (int fdx = 0; fdx < diffs.Length; fdx++)
            {
                Diff.Item item = diffs[fdx];

                // unchanged controls
                while ((n < item.StartB) && (n < currentInts.Length))
                {
                    currentHashes[currentInts[n]].BuildUpdateCommands(index, addedControls, commands);
                    n++;
                }

                // deleted controls
                var deletedIds = new List<string>();
                for (int m = 0; m < item.deletedA; m++)
                {
                    var deletedControl = previousHashes[previousInts[item.StartA + m]];
                    RemoveControlRecursively(index, deletedControl);
                    deletedIds.Add(deletedControl.Uid);
                }
                if (deletedIds.Count > 0)
                {
                    commands.Add($"remove {string.Join(" ", deletedIds)}");
                }

                // added controls
                while (n < item.StartB + item.insertedB)
                {
                    var cmd = currentHashes[currentInts[n]].GetCommandString(index: index, addedControls: addedControls);
                    commands.Add($"add to=\"{this.Uid}\" at=\"{n}\"\n{cmd}");
                    n++;
                }
            } // for

            // the rest of unchanged controls
            while (n < currentInts.Length)
            {
                currentHashes[currentInts[n]].BuildUpdateCommands(index, addedControls, commands);
                n++;
            }

            PreviousChildren.Clear();
            PreviousChildren.AddRange(currentChildren);
        }

        private void RemoveControlRecursively(Dictionary<string, Control> index, Control control)
        {
            // remove all children
            foreach (var child in control.GetChildren())
            {
                RemoveControlRecursively(index, child);
            }

            // remove control itself
            index.Remove(control.Uid);
        }

        internal string GetCommandString(string indent = "", Dictionary<string, Control> index = null, IList<Control> addedControls = null)
        {
            // remove control from index
            if (_uid != null && index != null)
            {
                index.Remove(_uid);
            }

            var lines = new List<string>();

            // main command
            var parts = new List<string>
            {
                // control name
                indent + ControlName
            };

            // base props
            var attrParts = GetCommandAttrs(update: false);
            parts.AddRange(attrParts);
            lines.Add(string.Join(" ", parts));

            if (addedControls != null)
            {
                addedControls.Add(this);
            }

            var children = GetChildren();
            if (children != null)
            {
                foreach(var control in children)
                {
                    var childCmd = control.GetCommandString(indent: indent + "  ", index: index, addedControls: addedControls);
                    if (childCmd != "")
                    {
                        lines.Add(childCmd);
                    }
                }
            }

            PreviousChildren.Clear();
            PreviousChildren.AddRange(children);

            return string.Join("\n", lines);
        }

        protected IList<string> GetCommandAttrs(bool update = false)
        {
            var parts = new List<string>();

            if (update && _uid == null)
            {
                return parts;
            }

            foreach(string attrName in _attrs.Keys.OrderBy(k => k))
            {
                var dirty = _attrs[attrName].IsDirty;
                if ((update && !dirty) || attrName == "id")
                {
                    continue;
                }

                var val = _attrs[attrName].Value.Encode();
                parts.Add($"{attrName}=\"{val}\"");
                _attrs[attrName].IsDirty = false;
            }

            if (!update && Id != null)
            {
                parts.Insert(0, $"id=\"{Id.Encode()}\"");
            }
            else if (update && parts.Count > 0)
            {
                parts.Insert(0, $"\"{_uid.Encode()}\"");
            }

            return parts;
        }
    }
}
