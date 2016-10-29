using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutomationEngine
{
    public class ApplicationMenuCollection : FileStorage<ApplicationMenuCollection>
    {
        public ApplicationMenuCollection()
        {
            Menus = new List<ApplicationMenuFileContext>();
        }

        public List<ApplicationMenuFileContext> Menus { get; set; }

        public override string StorageFileName => "application_menus.xml";

        public ApplicationMenuFileContext GetMenuByContext(string context)
        {
            return Menus.FirstOrDefault(x => Regex.IsMatch(context, x.ContextRegex));
        }
    }
}