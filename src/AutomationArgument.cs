using System;
using System.IO;

namespace AutomationEngine
{
    [Serializable]
    public class AutomationArgument : AbstractValue
    {
        private static BaseItem ItemWithOpenedContextMenu => MenuEngine.Instance.ItemWithOpenedContextMenu;

        public override string InteropValue
        {
            get
            {
                if (Value == "ActiveItemId")
                {
                    string itemId = ItemWithOpenedContextMenu.Id;
                    return "\"" + itemId + "\"";
                }
                if (Value == "ActiveMenuFilePath")
                {
                    string fileName = ItemWithOpenedContextMenu.ParentMenu.MenuFileName;
                    string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
                    return "\"" + filePath + "\"";
                }
                return Value;
            }
        }
    }
}