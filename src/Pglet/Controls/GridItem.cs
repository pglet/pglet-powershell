using System;
using System.Reflection;

namespace Pglet.Controls
{
    public partial class Grid
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
                foreach(var prop in _obj.GetType().GetProperties())
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
