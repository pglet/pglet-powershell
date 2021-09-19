using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;

namespace Pglet.Controls
{
    internal class GridHelper
    {
        private static string[] _reservedControlProperties = new string[] { "id" };

        internal static string EncodeReservedProperty(string name)
        {
            if (_reservedControlProperties.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                return "_" + name;
            }
            return name;
        }

        internal static string DecodeReservedProperty(string name)
        {
            if (name.StartsWith("_"))
            {
                var bareName = name.Substring(1);
                if (_reservedControlProperties.Contains(bareName, StringComparer.OrdinalIgnoreCase))
                {
                    return bareName;
                }
            }
            return name;
        }

        internal static ControlCollection<GridColumn> GenerateColumns(object obj)
        {
            var columns = new ControlCollection<GridColumn>();

            var otype = obj.GetType();

            var dict = new Dictionary<string, object>();

            if (otype.Name == "Hashtable")
            {
                var hashtable = obj as Hashtable;
                foreach (var propName in hashtable.Keys)
                {
                    var sname = propName.ToString();
                    var val = hashtable[propName];

                    columns.Add(new GridColumn
                    {
                        FieldName = sname,
                        Name = sname,
                        Resizable = true,
                        Sortable = val != null && IsNumber(val.GetType()) ? "number" : "string",
                        MaxWidth = 100
                    });

                    dict[propName.ToString()] = hashtable[propName];
                }
            }
            else if (otype.Name == "PSObject")
            {
                //var typeNames = (Collection<string>)GetPropertyValue(obj, "TypeNames");
                //Console.WriteLine("TypeNames: {0}", typeNames);

                IEnumerable<string> propNames = null;

                var propSet = GetPropertyValue(obj, "PSStandardMembers.Members.Item|DefaultDisplayPropertySet");
                if (propSet != null)
                {
                    propNames = (Collection<string>)GetPropertyValue(propSet, "ReferencedPropertyNames");
                }

                //Console.WriteLine("ReferencedPropertyNames: {0}", propNames);

                var props = GetPropertyValue(obj, "Properties");
                var objEnum = props.GetType().GetMethod("GetEnumerator").Invoke(props, null);

                var moveNext = objEnum.GetType().GetMethod("MoveNext");
                var current = objEnum.GetType().GetProperty("System.Collections.IEnumerator.Current", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                while ((bool)moveNext.Invoke(objEnum, null))
                {
                    var member = current.GetValue(objEnum);
                    var name = GetPropertyValue(member, "Name") as string;
                    var typeName = GetPropertyValue(member, "TypeNameOfValue") as string;

                    if (propNames == null || propNames.Contains(name))
                    {
                        columns.Add(new GridColumn
                        {
                            FieldName = name,
                            Name = name,
                            Resizable = true,
                            Sortable = IsNumber(Type.GetType(typeName)) ? "number" : "string",
                            MaxWidth = 100
                        });
                    }
                }
            }
            else
            {
                foreach (var prop in otype.GetProperties())
                {
                    columns.Add(new GridColumn
                    {
                        FieldName = prop.Name,
                        Name = prop.Name,
                        Resizable = true,
                        Sortable = IsNumber(prop.PropertyType) ? "number" : "string",
                        MaxWidth = 100
                    });
                }
            }

            return columns;
        }

        internal static object GetPropertyValue(object obj, string propName)
        {
            var propNames = propName.Split('.');
            object result = obj;
            foreach (var name in propNames)
            {
                var t = result.GetType();
                var pname = name;
                string idxName = null;

                var nameParts = name.Split('|');
                if (nameParts.Length > 1)
                {
                    pname = nameParts[0];
                    idxName = nameParts[1];
                }

                var p = t.GetProperty(pname, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null && idxName == null)
                {
                    result = p.GetValue(result);
                }
                else if (p != null && idxName != null)
                {
                    result = p.GetValue(result, new object[] { idxName });
                }
                else
                {
                    return null;
                }

                if (result == null)
                {
                    break;
                }
            }
            return result;
        }

        private static bool IsNumber(Type type)
        {
            return type == typeof(Int32) ||
                type == typeof(Int64) ||
                type == typeof(decimal) ||
                type == typeof(double) ||
                type == typeof(float);
        }
    }
}
