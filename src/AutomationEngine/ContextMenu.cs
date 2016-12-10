namespace AutomationEngine
{
    public class ContextMenu : Menu
    {
        public bool ApplicableToAllItems { get; set; }

        public ExecutableItemType ApplicableToItemType { get; set; }
    }
}