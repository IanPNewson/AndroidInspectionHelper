using INHelpers.Trees;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Android.Lib.Adb.UI
{

    public class Node : NodeContainer, ITree<Node>
    {

        public Node(XElement element, Node parent = null) : base(element)
        {
            Parent = parent;
        }

        public int Index
        {
            get => int.Parse(Element.Attribute("index").Value);
        }

        public string Id { get => Element.Attribute("resource-id").Value; }

        public string Text { get => Element.Attribute("text").Value; }

        public string Class { get => Element.Attribute("class").Value; }

        public string Package { get => Element.Attribute("package").Value; }

        public string ContentDescription { get => Element.Attribute("content-desc").Value; }

        public bool Checkable { get => bool.Parse(Element.Attribute("checkable").Value); }

        public bool Checked { get => bool.Parse(Element.Attribute("checked").Value); }

        public bool Clickable { get => bool.Parse(Element.Attribute("clickable").Value); }

        public bool Enabled { get => bool.Parse(Element.Attribute("enabled").Value); }

        public bool Focusable { get => bool.Parse(Element.Attribute("focusable").Value); }

        public bool Scrollable { get => bool.Parse(Element.Attribute("scrollable").Value); }

        public bool LongClickable { get => bool.Parse(Element.Attribute("long-clickable").Value); }

        public bool Password { get => bool.Parse(Element.Attribute("password").Value); }

        public bool Selected { get => bool.Parse(Element.Attribute("selected").Value); }

        public Rectangle Bounds
        {
            get
            {
                var bounds = Element.Attribute("bounds").Value.Split(new char[] { ',', '[', ']' }, System.StringSplitOptions.RemoveEmptyEntries)
                    .Select(str => int.Parse(str))
                    .ToArray();
                return new Rectangle(bounds[0], bounds[1], bounds[2] - bounds[0], bounds[3] - bounds[1]);
            }
        }

        protected override string GetDebuggerDisplay()
        {
            return $"{{ Id: {Id}, Class: {Class}, Children: {Children.Count()} }}";
        }

        #region tree

        Node ITree<Node>.Item => this;

        IEnumerable<ITree<Node>> ITree<Node>.Children => Children;

        public Node Parent { get; }

        #endregion

    }
}
