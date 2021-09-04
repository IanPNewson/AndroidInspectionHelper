using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android.Lib.Adb.UI
{
    public class NodeFilter
    {
        public Func<Node,bool> Filter { get; private set; }

        public NodeFilter BaseFilter { get; private set; }

        public bool DoesMatch(Node node)
        {
            return Filter(node) && (BaseFilter?.DoesMatch(node) ?? true);
        }

        public NodeFilter(Func<Node, bool> filter)
        {
            Filter = filter ?? throw new ArgumentNullException(nameof(filter));
        }

        protected static bool IsMatch(string test, string actual, StringMatchType matchType)
        {
            switch (matchType)
            {
                case StringMatchType.Exact:
                    return test == actual;
                case StringMatchType.CaseInsensitive:
                    return string.Equals(test, actual, StringComparison.OrdinalIgnoreCase);
                case StringMatchType.CaseInsensitivePartial:
                    return test.IndexOf(actual, StringComparison.OrdinalIgnoreCase) > -1;
            }
            throw new ArgumentOutOfRangeException(nameof(matchType));
        }

        public enum StringMatchType
        {
            Exact,
            CaseInsensitive,
            CaseInsensitivePartial
        }
    }

    public class NodePackageFilter : NodeFilter
    {
        public NodePackageFilter(string package, StringMatchType matchType) :
            base(node => IsMatch(package, node.Package, matchType))
        {
        }
    }

}
