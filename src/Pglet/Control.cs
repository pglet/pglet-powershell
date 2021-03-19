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
            return null;
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

            if (attrParts.Count > 0 && !update)
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

            return string.Join("\n", lines);
        }

        private IList<string> GetCommandAttrs(bool update = false)
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
