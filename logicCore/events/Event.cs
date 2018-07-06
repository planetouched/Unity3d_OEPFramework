using System.Collections.Generic;
using Assets.logicCore.contexts;
using Assets.logicCore.models;

namespace Assets.logicCore.events
{
    public class Event
    {
        internal class InnerComparer : IComparer<KeyValuePair<int, EventHandler>>
        {
            public int Compare(KeyValuePair<int, EventHandler> x, KeyValuePair<int, EventHandler> y)
            {
                if (x.Key > y.Key) return 1;
                if (x.Key < y.Key) return -1;
                return 0;
            }
        }

        private static readonly InnerComparer comparer = new InnerComparer();

        private static int globalAttachId;
        private int attachId;

        public delegate void EventHandler(CoreParams cp, IHandlerArgs args);

        readonly Dictionary<EventCategory, List<KeyValuePair<int, EventHandler>>> handlers = new Dictionary<EventCategory, List<KeyValuePair<int, EventHandler>>>();

        void InnerAttach(EventCategory category, EventHandler func)
        {
            List<KeyValuePair<int, EventHandler>> value;
            if (handlers.TryGetValue(category, out value))
                value.Add(new KeyValuePair<int, EventHandler>(attachId, func));
            else
            {
                value = new List<KeyValuePair<int, EventHandler>> { new KeyValuePair<int, EventHandler>(attachId, func) };
                handlers.Add(category, value);
            }
        }

        public void Attach(EventCategory category, EventHandler func)
        {
            attachId = globalAttachId++;
            InnerAttach(category, func);
        }

        void InnerDetach(EventCategory category, EventHandler func)
        {
            List<KeyValuePair<int, EventHandler>> list;
            if (handlers.TryGetValue(category, out list))
            {
                if (list.Count == 1)
                    list.RemoveAt(0);
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Value == func)
                        {
                            list.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        public void Detach(EventCategory category, EventHandler func)
        {
            InnerDetach(category, func);
        }

        public static void Call(EventCategory category, List<IModel> models, IHandlerArgs args, IContext context)
        {
            var toCall = new List<KeyValuePair<int, EventHandler>>();
            foreach (var model in models)
            {
                List<KeyValuePair<int, EventHandler>> tmp;
                if (model.GetEvent().handlers.TryGetValue(category, out tmp))
                    toCall.AddRange(model.GetEvent().handlers[category]);
            }

            if (toCall.Count == 0) return;

            if (toCall.Count > 1)
                toCall.Sort(comparer);

            var eventCallStack = new EventCallStack();
            eventCallStack.Set(models, true);

            CoreParams cp;
            cp.stack = eventCallStack;
            cp.context = context;
            cp.category = category;

            switch (toCall.Count)
            {
                case 1:
                    toCall[0].Value(cp, args);
                    break;
                case 2:
                    {
                        var c0 = toCall[0];
                        var c1 = toCall[1];
                        c0.Value(cp, args);
                        c1.Value(cp, args);
                        break;
                    }
                case 3:
                    {
                        var c0 = toCall[0];
                        var c1 = toCall[1];
                        var c2 = toCall[2];
                        c0.Value(cp, args);
                        c1.Value(cp, args);
                        c2.Value(cp, args);
                        break;
                    }
                default:
                    {
                        var cpy = new KeyValuePair<int, EventHandler>[toCall.Count];
                        toCall.CopyTo(cpy, 0);
                        foreach (var pair in cpy)
                            pair.Value(cp, args);
                        break;
                    }
            }
        }

        public void Clear()
        {
            handlers.Clear();
        }
    }
}
