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
                    return $"\"{GetExecutingItemId()}\"";
                }
                if (Value == "ActiveItemName")
                {
                    return $"\"{GetExecutingItemName()}\"";
                }
                if (Value == "ActiveMenuFilePath")
                {
                    string fileName = ItemWithOpenedContextMenu.ParentMenu.MenuFileName;
                    string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
                    return $"\"{filePath}\"";
                }
                return Value;
            }
        }

        private string GetExecutingItemId()
        {
            string replacedItemId = (ItemWithOpenedContextMenu as ExecutableItem)?.ReplacedItemId;
            if (!string.IsNullOrWhiteSpace(replacedItemId))
            {
                return replacedItemId;
            }
            return ItemWithOpenedContextMenu.Id;
        }

        private string GetExecutingItemName()
        {
            var executableItem = MenuEngine.Instance.SelectedItem as ExecutableItem;

            string name = executableItem?.Name;
            if (name == null)
            {
                return null;
            }

            string prefix = executableItem?.ParentMenu?.Name;
            if (prefix != null && name.StartsWith(prefix))
            {
                return name.Substring(prefix.Length).TrimStart();
            }

            return name;
        }
    }
}