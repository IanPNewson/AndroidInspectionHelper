using System;
using System.Text.RegularExpressions;

namespace Android.Lib.Adb
{

    /// <summary>
    /// When given a POCO as a type and a regex with named groups, will return an instance of that class
    /// with the properties set to the values found by the regex
    /// </summary>
    public class TypedReceiver<TOutput> : StringReceiver
        where TOutput : new()
    {

        public TypedReceiver(Regex rgx, Action<TOutput> action) : base(new Action<string>(str =>
        {
            var match = rgx.Match(str);
            var instance = new TOutput();
            foreach (Group group in match.Groups)
            {
                var property = typeof(TOutput).GetProperty(group.Name);
                if (null != property)
                    property.SetValue(instance, Convert.ChangeType(group.Value, property.PropertyType));
            }
            action?.Invoke(instance);
        }))
        {
        }

    }

}
