using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace Android.Lib.Adb.UI
{

    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class NodeContainer
    {
        protected XElement Element { get; private set; }

        public NodeContainer(XElement element)
        {
            Element = element ?? throw new ArgumentNullException(nameof(element));
        }

        public NodeFilter Filter { get; private set; }

        public IEnumerable<Node> Children
        {
            get
            {
                var nodes = Element.Elements()
                                    .Select(el => new Node(el, this as Node));
                if (null != Filter)
                    nodes = nodes.Where(n => Filter.DoesMatch(n));
                return nodes;
            }
        }

        protected virtual string GetDebuggerDisplay()
        {
            return $"{{ Children: {Children.Count()} }}";
        }

    }
}
