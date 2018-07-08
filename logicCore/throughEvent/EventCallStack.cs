using System;
using System.Collections.Generic;

namespace Assets.logicCore.throughEvent
{
    public class EventCallStack
    {
        private Dictionary<Type, IEventSource> stack;
        private List<IEventSource> path;

        public void Set(List<IEventSource> path, bool reverse)
        {
            this.path = new List<IEventSource>(path);
            if (reverse)
                this.path.Reverse();
        }

        public T GetSelf<T>() where T : IEventSource
        {
            return (T)GetSelf();
        }

        public IEventSource GetSelf()
        {
            return path[path.Count - 1];
        }

        public T Get<T>() where T : class
        {
            if (stack == null)
            {
                stack = new Dictionary<Type, IEventSource>();
                for (int i = 0; i < path.Count - 1; i++)
                {
                    stack.Add(path[i].GetType(), path[i]);
                }
            }
            return (T)stack[typeof(T)];
        }
    }
}
