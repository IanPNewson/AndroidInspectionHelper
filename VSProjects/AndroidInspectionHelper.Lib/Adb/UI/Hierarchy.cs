using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Android.Lib.Adb.UI
{
    
    public class Hierarchy : NodeContainer
    {

        public Hierarchy(XElement element) : base(element)
        {
        }

        public Orientations Rotation
        {
            get => (Orientations)int.Parse(Element.Attribute("rotation").Value);
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    Children.Min(node => node.Bounds.X),
                    Children.Min(node => node.Bounds.Y),
                    Children.Max(node => node.Bounds.Width),
                    Children.Max(node => node.Bounds.Height)
                    );
            }
        }

    }
}
