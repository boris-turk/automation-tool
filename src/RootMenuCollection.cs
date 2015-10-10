using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [XmlRoot("Menus")]
    public class RootMenuCollection
    {
        [XmlElement("Menu")]
        public List<Menu> Menus { get; set; }

        public RootMenuCollection()
        {
            Menus = new List<Menu>();
        }
    }
}
