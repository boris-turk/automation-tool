using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class ReferencedMenu : Menu
    {
        public ReferencedMenu()
        {
            NameParts = new List<NamePart>();
        }

        [XmlAttribute]
        public bool MergedIntoParent { get; set; }

        [XmlArrayItem("Part")]
        public List<NamePart> NameParts { get; set; }

        public string GetProperItemName(BaseItem item, BaseItem original)
        {
            if (!NameParts.Any())
            {
                return original.Name;
            }

            string name = string.Empty;
            foreach (NamePart namePart in NameParts)
            {
                if (namePart.Type == NamePartType.Constant)
                {
                    name += namePart.Value + " ";
                }
                if (namePart.Type == NamePartType.ReferencedMenu)
                {
                    name += original.Name + " ";
                }
            }

            return name.TrimEnd();
        }

    }
}