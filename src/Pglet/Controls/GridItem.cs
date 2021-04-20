using System;
using System.Collections;
using System.Collections.Generic;
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

        internal override void SetAttr(string name, string value, bool dirty = true)
        {
            base.SetAttr(name, value, false);

            var prop = _obj.GetType().GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                prop.SetValue(_obj, Convert.ChangeType(value, prop.PropertyType), null);
            }
        }

        internal void FetchAttrs()
        {
            var otype = _obj.GetType();

            var dict = new Dictionary<string, object>();
            
            if (otype.Name == "Hashtable")
            {
                var hashtable = _obj as Hashtable;
                foreach(var propName in hashtable.Keys)
                {
                    dict[propName.ToString()] = hashtable[propName];
                }
            }
            else if (otype.Name == "PSObject")
            {
                var props = otype.GetProperty("Properties").GetValue(_obj);
                var objEnum = props.GetType().GetMethod("GetEnumerator").Invoke(props, null);

                var moveNext = objEnum.GetType().GetMethod("MoveNext");
                var current = objEnum.GetType().GetProperty("System.Collections.IEnumerator.Current", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                while ((bool)moveNext.Invoke(objEnum, null))
                {
                    var member = current.GetValue(objEnum);
                    var name = member.GetType().GetProperty("Name").GetValue(member) as string;
                    var val = member.GetType().GetProperty("Value").GetValue(member);

                    dict[name] = val;
                }
            }
            else
            {
                foreach (var prop in otype.GetProperties())
                {
                    dict[prop.Name] = prop.GetValue(_obj);
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
                        base.SetAttr(propName, sval, dirty: true);
                    }
                }
            }
        }
    }
}
