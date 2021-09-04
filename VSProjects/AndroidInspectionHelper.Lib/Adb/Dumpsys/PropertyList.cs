using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Android.Lib.Adb.Dumpsys
{

    public class PropertyList
    {

        public static PropertyList Parse(string src, string indetationMark)
        {
            var props = new Stack<PropertyList>();
            props.Push(new PropertyList());
            using (var reader = new StringReader(src))
            {
                string line;
                while (null != (line = reader.ReadLine()))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var depth = 0;
                    while (line.StartsWith(indetationMark))
                    {
                        ++depth;
                        line = line.Substring(indetationMark.Length);
                    }
                    if (depth > props.Count)
                        throw new ArgumentException($"Invalid list at line '{line}'. Depth is {depth} but we're only {props.Count} deep.", nameof(src));

                    while (props.Count > depth + 1)
                    {
                        props.Pop();
                    }

                    props.Peek().Values.Add(line);

                    var propertyName = line;
                    var newProps = new PropertyList();

                    if (!props.Peek().Properties.ContainsKey(propertyName))
                    {
                        props.Peek().Properties.Add(propertyName, newProps);
                        props.Push(newProps);
                    }
                }
            }

            var root = props.Last();

            RemoveEmptyPropertyLists(root);

            return root;

            void RemoveEmptyPropertyLists(PropertyList props)
            {
                foreach (var subPropsKey in props.Properties.Keys)
                {
                    if (!props.Properties[subPropsKey].Properties.Any())
                    {
                        props.Properties.Remove(subPropsKey);
                    }
                    else
                    {
                        RemoveEmptyPropertyLists(props.Properties[subPropsKey]);
                    }
                }
            }
        }

        public Dictionary<string, PropertyList> Properties { get; private set; } = new Dictionary<string, PropertyList>();

        public List<string> Values { get; private set; } = new List<string>();

        public PropertyList this[string propertyName]
        {
            get
            {
                if (Properties.ContainsKey(propertyName))
                    return Properties[propertyName];
                return null;
            }
        }

    }

}
