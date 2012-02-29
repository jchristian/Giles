using System.Collections.Generic;
using Giles.Core.UI;
using Growl.CoreLibrary;

namespace Giles.Specs.Core.UI
{
    public class FakeGrowlAdapter : IGrowlAdapter
    {
        public IList<string> Messages = new List<string>();

        public void Notify(string id, string title, string text)
        {
            Messages.Add(text);
        }

        public void Notify(string id, string title, string text, Resource icon)
        {
            Messages.Add(text);
        }
    }
}