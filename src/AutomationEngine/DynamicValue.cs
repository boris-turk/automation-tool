using System;
using System.IO;

namespace AutomationEngine
{
    [Serializable]
    public class DynamicValue : AbstractValue
    {
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
                    string fileName = MenuEngine.Instance.ItemWithOpenedContextMenu.ParentMenu.MenuFileName;
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