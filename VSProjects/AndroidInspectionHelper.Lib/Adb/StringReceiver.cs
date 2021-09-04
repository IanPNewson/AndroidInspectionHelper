using SharpAdbClient;
using System;
using System.Text;

namespace Android.Lib.Adb
{
    public class StringReceiver : IShellOutputReceiver
    {

        private StringBuilder _Output = new StringBuilder();

        public StringReceiver(Action<string> action)
        {
            Action = action;
        }

        public bool ParsesErrors => throw new NotImplementedException();

        public Action<string> Action { get; }

        public void AddOutput(string line)
        {
            _Output.AppendLine(line);
        }

        public void Flush()
        {
            Action?.Invoke(_Output.ToString());
        }

        public static implicit operator StringReceiver(Action<string> action)
        {
            return new StringReceiver(action);
        }

    }

}
