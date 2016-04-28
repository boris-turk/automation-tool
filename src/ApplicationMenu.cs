using System.Collections.Generic;

namespace AutomationEngine
{
    public class ApplicationMenu : Menu
    {
        public static Menu DefaultApplicationMenu
        {
            get
            {
                BaseItem item = new ExecutableItem
                {
                    Name = "Create",
                    ActionType = ActionType.CreateApplicationMenu
                };

                var menu = new Menu();
                menu.Items.Add(item);

                return menu;
            }
        }

        private ExecutableItem AddMenuItem
        {
            get
            {
                var item = new ExecutableItem
                {
                    Name = "add menu item",
                    ExecutingMethodName = "Execute"
                };
                item.Arguments.Add(new StringValue { Value = MenuFileName });
                return item;
            }
        }

        public override IEnumerable<BaseItem> GetSelectableItems()
        {
            foreach (BaseItem item in base.GetSelectableItems())
            {
                yield return item;
            }
            yield return AddMenuItem;
        }

        public override void SaveToFile()
        {
            Items.Remove(AddMenuItem);
            base.SaveToFile();
        }
    }
}