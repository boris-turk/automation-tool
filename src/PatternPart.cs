﻿using System;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public abstract class PatternPart
    {
        [XmlAttribute]
        public string Value { get; set; }

        public bool ValueSpecified
        {
            get { return !string.IsNullOrWhiteSpace(Value); }
        }

        public abstract string DisplayValue { get; }

        public abstract bool IsMatch(string text);
    }
}