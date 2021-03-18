using System;
using System.Collections.Generic;

namespace Pglet
{
    public class Control
    {
        public class AttrValue
        {
            public string Value { get; set; }
            public bool IsDirty { get; set; }
        }

        private Dictionary<string, AttrValue> _attrs = new Dictionary<string, AttrValue>(StringComparer.InvariantCultureIgnoreCase);

        public string Id
        {
            get { return GetAttr("id"); }
            set { SetAttr("id", value); }
        }

        public Control(string id = null, string width = null, string height = null, string padding = null, string margin = null,
            bool? visible = null, bool? disabled = null)
        {

        }

        protected void SetAttr(string name, bool value)
        {
            SetAttr(name, value.ToString().ToLowerInvariant());
        }

        protected void SetAttr(string name, string value)
        {
            if (value != null)
            {
                _attrs[name] = new AttrValue { Value = value, IsDirty = true };
            }
        }

        protected bool GetBoolAttr(string name, bool defValue = false)
        {
            return _attrs.ContainsKey(name) ? Boolean.Parse(_attrs[name].Value) : defValue;
        }

        protected string GetAttr(string name, string defValue = null)
        {
            return _attrs.ContainsKey(name) ? _attrs[name].Value : defValue;
        }
    }
}
