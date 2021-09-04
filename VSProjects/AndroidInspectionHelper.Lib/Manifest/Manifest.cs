using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Android.Lib.Manifest
{
    public class Manifest : ManifestElement
    {

        public static Manifest FromXml(string xml)
        {
            var doc = XDocument.Parse(xml);
            return new Manifest(doc.Root);
        }

        public Manifest(XElement element) : base(element)
        {
        }

        public IEnumerable<Permission> Permissions { get => Element.Elements("uses-permission").Select(element => new Permission(element)); }
    }

    public class Permission : ManifestElement
    {
        public Permission(XElement element) : base(element)
        {
        }

        public string Name { get => Element.Attribute(XName.Get("name", NS_ANDROID)).Value; }
    }

    public abstract class ManifestElement
    {

        public const string NS_ANDROID = "http://schemas.android.com/apk/res/android";
        protected XElement Element { get; private set; }

        protected ManifestElement(XElement element)
        {
            Element = element ?? throw new ArgumentNullException(nameof(element));
        }
    }
}
