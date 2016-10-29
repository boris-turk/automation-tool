using System.Windows.Forms;
using System.Xml.Serialization;

namespace AutomationEngine
{
    public class Shortcut
    {
        private char _character;

        [XmlAttribute]
        public bool Control { get; set; }

        public bool ControlSpecified => Control;

        [XmlAttribute]
        public bool Alt { get; set; }

        public bool AltSpecified => Alt;

        [XmlIgnore]
        public char Character
        {
            get { return _character; }
            set { _character = char.ToUpperInvariant(value); }
        }

        [XmlElement("Character")]
        public string SerializationText
        {
            get { return Character.ToString(); }
            set
            {
                char[] charArray = value.ToCharArray();
                if (charArray.Length == 1)
                {
                    Character = charArray[0];
                }
            }
        }

        public bool SerializationTextSpecified => Character != 0;

        [XmlElement("Key")]
        public Keys Key { get; set; }

        public bool KeySpecified => Key != Keys.None;
    }
}