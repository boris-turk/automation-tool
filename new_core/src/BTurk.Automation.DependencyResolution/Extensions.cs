using System;
using System.Linq;

namespace BTurk.Automation.DependencyResolution
{
    public static class Extensions
    {
        public static bool InheritsFrom(this Type type, Type parent)
        {
            if (parent.IsAssignableFrom(type))
            {
                return true;
            }

            while (type != null && type != typeof(object))
            {
                Type current = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (parent == current)
                {
                    return true;
                }
                if (parent.IsInterface)
                {
                    var interfaces = type.GetInterfaces().Where(x => x.IsGenericType);
                    return interfaces.Any(x => x.GetGenericTypeDefinition() == parent);
                }
                type = type.BaseType;
            }
            return false;
        }
    }
}