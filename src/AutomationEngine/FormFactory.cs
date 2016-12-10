using System;
using System.Collections.Generic;

namespace AutomationEngine
{
    public static class FormFactory
    {
        private static readonly Dictionary<Type, object> Instances = new Dictionary<Type, object>();

        public static T Instance<T>() where T : AutomationEngineForm, new()
        {
            object instance;

            if (Instances.TryGetValue(typeof(T), out instance))
            {
                return (T)instance;
            }

            T t = new T();
            Instances.Add(typeof(T), t);
            return t;
        }
    }
}