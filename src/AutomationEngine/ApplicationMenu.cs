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
    }
}