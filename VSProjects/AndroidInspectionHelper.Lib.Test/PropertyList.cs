using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AndroidLib.Test
{
    public partial class DumpSysPropertyListParseTest_
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

                        if (line.EndsWith(':'))
                        {
                            var propertyName = line.Substring(0, line.Length - 1);
                            var newProps = new PropertyList();
                            props.Peek().Properties.Add(propertyName, newProps);
                            props.Push(newProps);
                        }
                        else
                        {
                            props.Peek().Values.Add(line);
                        }
                    }
                }
                
                return props.Last();
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
}
