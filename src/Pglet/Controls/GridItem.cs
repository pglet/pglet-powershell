using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pglet.Controls
{
    public class GridItem : Control
    {
        protected override string ControlName => "item";

        object _obj;

        public object Obj
        {
            get { return _obj; }
        }

        public GridItem(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            _obj = obj;
        }

        internal override void SetAttrInternal(string name, string value, bool dirty = true)
        {
            base.SetAttrInternal(name, value, false);

            var prop = _obj.GetType().GetProperty(GridHelper.DecodeReservedProperty(name), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                prop.SetValue(_obj, Convert.ChangeType(value, prop.PropertyType), null);
            }
        }

        internal void FetchAttrs(IEnumerable<string> fetchPropNames, string keyFieldName)
        {
            var otype = _obj.GetType();

            var dict = new Dictionary<string, object>();

            if (otype.Name == "Hashtable")
            {
                var hashtable = _obj as Hashtable;
                foreach (var propName in hashtable.Keys)
                {
                    var sprop = propName.ToString();
                    if (fetchPropNames == null || fetchPropNames.Contains(sprop))
                    {
                        dict[sprop] = hashtable[propName];
                    }
                }
            }
            else if (otype.Name == "PSObject")
            {
                var props = GridHelper.GetPropertyValue(_obj, "Properties");
                var objEnum = props.GetType().GetMethod("GetEnumerator").Invoke(props, null);

                var moveNext = objEnum.GetType().GetMethod("MoveNext");
                var current = objEnum.GetType().GetProperty("System.Collections.IEnumerator.Current", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                while ((bool)moveNext.Invoke(objEnum, null))
                {
                    var member = current.GetValue(objEnum);
                    var name = GridHelper.GetPropertyValue(member, "Name") as string;
                    if (fetchPropNames == null || fetchPropNames.Contains(name))
                    {
                        object val = null;
                        try
                        {
                            val = GridHelper.GetPropertyValue(member, "Value");
                        }
                        catch { }
                        dict[name] = val;
                    }
                }
            }
            else
            {
                foreach (var prop in otype.GetProperties())
                {
                    var sprop = prop.Name;
                    if (fetchPropNames == null || fetchPropNames.Contains(sprop))
                    {
                        object val = null;
                        try
                        {
                            val = prop.GetValue(_obj);
                        }
                        catch { }
                        dict[sprop] = val;
                    }
                }
            }

            foreach (var propName in dict.Keys)
            {
                var val = dict[propName];
                if (val != null)
                {
                    var sval = val.ToString();

                    if (val.GetType() == typeof(bool))
                    {
                        sval = sval.ToLowerInvariant();
                    }

                    var origSval = this.GetAttr(propName);

                    if (sval != origSval)
                    {
                        if (String.Equals(keyFieldName, propName, StringComparison.OrdinalIgnoreCase))
                        {
                            this._id = sval;
                        }
                        base.SetAttr(GridHelper.EncodeReservedProperty(propName), sval, dirty: true);
                    }
                }
            }
        }
    }
}
