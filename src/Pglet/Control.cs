using System;
using System.Collections.Generic;
using System.Linq;

namespace Pglet
{
    public abstract class Control
    {
        public class AttrValue
        {
            public string Value { get; set; }
            public bool IsDirty { get; set; }
        }

        private Dictionary<string, AttrValue> _attrs = new Dictionary<string, AttrValue>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, EventHandler> _eventHandlers = new Dictionary<string, EventHandler>(StringComparer.OrdinalIgnoreCase);
        private List<Control> _previousChildren = new List<Control>(); // hash codes of previous children
        private string _id;
        private Connection _conn;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
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

        internal Dictionary<string, EventHandler> EventHandlers
        {
            get { return _eventHandlers; }
        }

        protected abstract string ControlName { get; }
        protected virtual IEnumerable<Control> GetChildren()
        {
            return new Control[] {};
        }

        internal List<Control> PreviousChildren
        {
            get { return _previousChildren; }
        }

        public Control(string id = null, string width = null, string height = null, string padding = null, string margin = null,
            bool? visible = null, bool? disabled = null)
        {
            Id = id;
            Width = width;
            Height = height;
            Padding = padding;
            Margin = margin;
            if (visible.HasValue)
            {
                Visible = visible.Value;
            }
            if (disabled.HasValue)
            {
                Disabled = disabled.Value;
            }
        }

        protected void AddEventHandler(string eventName, EventHandler handler)
        {
            _eventHandlers[eventName] = handler;

            if (_conn != null)
            {
                _conn.AddEventHandler(this._id, eventName, handler);
            }
        }

        protected void SetBoolAttr(string name, bool value)
        {
            SetAttr(name, value.ToString().ToLowerInvariant());
        }

        protected bool GetBoolAttr(string name, bool defValue = false)
        {
            return _attrs.ContainsKey(name) ? Boolean.Parse(_attrs[name].Value) : defValue;
        }

        protected void SetAttr(string name, string value)
        {
            if (value != null)
            {
                _attrs[name] = new AttrValue { Value = value, IsDirty = true };
            }
        }

        protected string GetAttr(string name, string defValue = null)
        {
            return _attrs.ContainsKey(name) ? _attrs[name].Value : defValue;
        }

        internal void BuildUpdateCommands(List<Control> index, List<string> commands)
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
                    currentHashes[currentInts[n]].BuildUpdateCommands(index, commands);
                    n++;
                }

                // deleted controls
                for (int m = 0; m < item.deletedA; m++)
                {
                    var deletedControl = previousHashes[previousInts[item.StartA + m]];
                    commands.Add($"remove {deletedControl.Id}");
                }

                // added controls
                while (n < item.StartB + item.insertedB)
                {
                    var cmd = currentHashes[currentInts[n]].GetCommandString(index: index, conn: _conn);
                    commands.Add($"add to=\"{this.Id}\" at=\"{n}\"\n{cmd}");
                    n++;
                }
            } // for

            // the rest of unchanged controls
            while (n < currentInts.Length)
            {
                currentHashes[currentInts[n]].BuildUpdateCommands(index, commands);
                n++;
            }

            PreviousChildren.Clear();
            PreviousChildren.AddRange(currentChildren);
        }

        internal string GetCommandString(bool update = false, string indent = "", IList<Control> index = null, Connection conn = null)
        {
            _conn = conn;

            if (!update)
            {
                // reset ID
                if (_id != null && _id.Split(':').Last().StartsWith("_"))
                {
                    _id = null;
                }
                else if (_id != null)
                {
                    _id = _id.Split(':').Last();
                }
            }

            var lines = new List<string>();

            // main command
            var parts = new List<string>();

            if (!update)
            {
                parts.Add(indent + ControlName);
            }

            // base props
            var attrParts = GetCommandAttrs(update);

            if (attrParts.Count > 0 || !update)
            {
                parts.AddRange(attrParts);
                lines.Add(string.Join(" ", parts));
            }

            if (index != null)
            {
                index.Add(this);
            }

            var children = GetChildren();
            if (children != null)
            {
                foreach(var control in children)
                {
                    var childCmd = control.GetCommandString(update: update, indent: indent + "  ", index: index);
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

            if (update && _id == null)
            {
                return parts;
            }

            foreach(string attrName in _attrs.Keys.OrderBy(k => k))
            {
                var dirty = _attrs[attrName].IsDirty;
                if (update && !dirty)
                {
                    continue;
                }

                var val = _attrs[attrName].Value.Encode();
                parts.Add($"{attrName}=\"{val}\"");
                _attrs[attrName].IsDirty = false;
            }

            if (_id != null)
            {
                if (!update)
                {
                    parts.Insert(0, $"id=\"{_id.Encode()}\"");
                }
                else if (parts.Count > 0)
                {
                    parts.Insert(0, $"\"{_id.Encode()}\"");
                }
            }

            return parts;
        }
    }
}
