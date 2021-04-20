using System;
using System.Collections;
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
            
            if (otype.Name == "PSObject")
            {
                var propsMember = otype.GetProperty("Properties");
                var props = propsMember.GetValue(_obj);

                var getEnum = props.GetType().GetMethod("GetEnumerator");
                var objEnum = getEnum.Invoke(props, null);

                var moveNext = objEnum.GetType().GetMethod("MoveNext");
                var current = objEnum.GetType().GetProperty("System.Collections.IEnumerator.Current", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                while ((bool)moveNext.Invoke(objEnum, null))
                {
                    var member = current.GetValue(objEnum);
                    var nameProp = member.GetType().GetProperty("Name");
                    var valueProp = member.GetType().GetProperty("Value");

                    var name = nameProp.GetValue(member) as string;
                    var val = valueProp.GetValue(member);
                    if (val != null)
                    {
                        var sval = val.ToString();

                        if (val.GetType() == typeof(bool))
                        {
                            sval = sval.ToLowerInvariant();
                        }

                        var origSval = this.GetAttr(name);

                        if (sval != origSval)
                        {
                            base.SetAttr(name, sval, dirty: true);
                        }
                    }
                }
            } else
            {
                foreach (var prop in otype.GetProperties())
                {
                    var val = prop.GetValue(_obj);
                    if (val != null)
                    {
                        var sval = val.ToString();

                        if (prop.PropertyType == typeof(bool))
                        {
                            sval = sval.ToLowerInvariant();
                        }

                        var origSval = this.GetAttr(prop.Name);

                        if (sval != origSval)
                        {
                            base.SetAttr(prop.Name, sval, dirty: true);
                        }
                    }
                }
            }
        }
    }
}
