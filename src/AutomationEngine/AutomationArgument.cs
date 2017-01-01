using System;
using System.IO;

namespace AutomationEngine
{
    [Serializable]
    public class AutomationArgument : AbstractValue
    {
        public override string InteropValue
        {
            get
            {
                if (Value == "ActiveItemId")
                {
                    return $"\"{MenuEngine.Instance.GetExecutingItemId()}\"";
                }
                if (Value == "ActiveItemName")
                {
                    return $"\"{MenuEngine.Instance.GetExecutingItemName()}\"";
                }
                if (Value == "ActiveMenuFilePath")
                {
                    string fileName = MenuEngine.Instance.ItemWithOpenedContextMenu.ParentMenu.MenuFileName;
                    string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
                    return $"\"{filePath}\"";
                }
                return Value;
            }
        }
    }
}