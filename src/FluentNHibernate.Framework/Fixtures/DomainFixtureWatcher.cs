using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.Framework.Fixtures
{
    public static class DomainFixtureWatcher
    {
        private static List<Action<object>> list = new List<Action<object>>();
        private static Dictionary<Type, List<Delegate>> actionsByType = new Dictionary<Type, List<Delegate>>();

        public static Action<Type> LoadingList = t => { };

        public static void RegisterListener(Action<object> action)
        {
            list.Add(action);
        }

        public static void RegisterListenerForType<T>(Action<T> action)
        {
            List<Delegate> actionList;

            if (!actionsByType.TryGetValue(typeof(T), out actionList))
            {
                actionList = new List<Delegate>();
                actionsByType.Add(typeof(T), actionList);
            }

            actionList.Add(action);
        }

        public static void Added(object target)
        {
            foreach (var action in list)
            {
                action(target);
            }

            if (target != null)
            {
                List<Delegate> actions;

                if (actionsByType.TryGetValue(target.GetType(), out actions))
                {
                    foreach (Delegate action in actions)
                    {
                        action.DynamicInvoke(target);
                    }
                }
            }
        }

        public static void LoadingListOfType<T>()
        {
            LoadingList(typeof (T));
        }
    }
}