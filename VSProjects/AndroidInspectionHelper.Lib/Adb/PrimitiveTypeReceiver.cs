using System;

namespace Android.Lib.Adb
{
    public class PrimitiveTypeReceiver<T> : StringReceiver
        where T : struct
    {
        public T Value { get; private set; }

        public PrimitiveTypeReceiver(Action<T> action) : base(str =>
        {
            action((T)Convert.ChangeType(str, typeof(T)));
        })
        {
        }
    }

}
