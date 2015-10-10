﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ahk
{
    [XmlRoot("ExecutableItems")]
    public class ExecutableItemsCollection
    {
        [XmlElement("Item")]
        public List<ExecutableItem> Items { get; set; }

        public ExecutableItemsCollection()
        {
            Items = new List<ExecutableItem>();
        }
    }
}