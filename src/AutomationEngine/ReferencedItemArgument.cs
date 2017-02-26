using System.Linq;
using System.Security.AccessControl;
using System.Xml.Serialization;

namespace AutomationEngine
{
    public class ReferencedItemArgument : AbstractValue
    {
        public override bool IsEmpty => string.IsNullOrWhiteSpace(InteropValue);

        [XmlAttribute]
        public ValueType TypeFilter { get; set; }

        public override string InteropValue
        {
            get
            {
                var item = MenuEngine.Instance.ItemWithOpenedContextMenu as ExecutableItem;
                if (item == null)
                {
                    return null;
                }
                AbstractValue argument = GetMatchingArgument(item);
                return argument?.InteropValue;
            }
        }

        private AbstractValue GetMatchingArgument(ExecutableItem item)
        {
            foreach (AbstractValue argument in item.Arguments)
            {
                if (argument.Type == TypeFilter)
                {
                    return argument;
                }
                for (int i = 0; i < item.Arguments.Count; i++)
                {
                    ValueType argumentType = GetArgumentType(item, i);
                    if (argumentType == TypeFilter)
                    {
                        return argument;
                    }
                }
            }
            return null;
        }

        private ValueType GetArgumentType(ExecutableItem item, int argumentIndex)
        {
            foreach (Menu menu in item.GetParentMenus())
            {
                if (menu.ArgumentTypes != null && argumentIndex < menu.ArgumentTypes.Count)
                {
                    return menu.ArgumentTypes[argumentIndex];
                }
            }
            return ValueType.None;
        }
    }
}