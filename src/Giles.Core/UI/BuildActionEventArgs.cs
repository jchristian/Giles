using System;

namespace Giles.Core.UI
{
    public class BuildActionEventArgs : EventArgs
    {
        public string Message { get; set; }
        public object[] Parameters { get; set; }

        public BuildActionEventArgs(string message) : this(message, new object[] { })
        { }

        public BuildActionEventArgs(string message, params object[] parameters)
        {
            Message = message;
            Parameters = parameters;
        }
    }
}