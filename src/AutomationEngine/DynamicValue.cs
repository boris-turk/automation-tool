using System;
using System.IO;

namespace AutomationEngine
{
    [Serializable]
    public class DynamicValue : AbstractValue
    {
        private BaseItem ItemWithOpenedContextMenu => MenuEngine.Instance.ItemWithOpenedContextMenu;

        public override string InteropValue
        {
            get
            {
                if (Value == "ActiveItemId")
                {
                    return $"\"{MenuEngine.Instance.GetExecutingItemId()}\"";
                }
                if (Value == "ActiveMenuFilePath")
                {
                    string fileName = ItemWithOpenedContextMenu?.PersistenceParentMenu?.MenuFileName;
                    if (fileName == null)
                    {
                        return null;
                    }
                    string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
                    return $"\"{filePath}\"";
                }
                return Value;
            }
        }

        public override bool IsEmpty => string.IsNullOrWhiteSpace(InteropValue);
    }
}