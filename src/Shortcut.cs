using System;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AutomationEngine
{
    public class Shortcut
    {
        private const string ControlIdentifier = "Control+";
        private const string AltIdentifier = "Alt+";

        private char _key;

        [XmlIgnore]
        public bool Control { get; set; }

        [XmlIgnore]
        public bool Alt { get; set; }

        [XmlIgnore]
        public char Key
        {
            get { return _key; }
            set { _key = char.ToUpperInvariant(value); }
        }

        [XmlText]
        public string SerializationText
        {
            get
            {
                StringBuilder value = new StringBuilder();
                if (Control)
                {
                    value.Append(ControlIdentifier);
                }
                if (Alt)
                {
                    value.Append(AltIdentifier);
                }
                value.Append(Key);
                return value.ToString();
            }
            set
            {
                ParseValue(value);
            }
        }

        public bool SerializationTextSpecified
        {
            get { return Key > 0; }
        }

        public Keys KeyData { get; set; }

        public bool KeyDataSpecified
        {
            get { return KeyData != Keys.None; }
        }

        private void ParseValue(string value)
        {
            Alt = false;
            Control = false;
            Key = (char)0;

            int index = value.IndexOf(ControlIdentifier, StringComparison.Ordinal);
            if (index >= 0)
            {
                Control = true;
                value = value.Remove(index, ControlIdentifier.Length);
            }

            index = value.IndexOf(AltIdentifier, StringComparison.Ordinal);
            if (index >= 0)
            {
                Alt = true;
                value = value.Remove(index, AltIdentifier.Length);
            }

            char[] charArray = value.ToCharArray();
            if (charArray.Length == 1)
            {
                Key = charArray[0];
            }
        }
    }
}