using System;
using System.Collections.Generic;
using Assets.logicCore.models;

namespace Assets.logicCore.events
{
    public class EventCallStack
    {
        private Dictionary<Type, IModel> stack;
        private List<IModel> models;

        public void Set(List<IModel> modelList, bool reverse)
        {
            models = new List<IModel>(modelList);
            if (reverse)
                models.Reverse();
        }

        public T GetSelf<T>() where T : IModel
        {
            return (T)GetSelf();
        }

        public IModel GetSelf()
        {
            return models[models.Count - 1];
        }

        public T Get<T>() where T : class
        {
            if (stack == null)
            {
                stack = new Dictionary<Type, IModel>();
                for (int i = 0; i < models.Count - 1; i++)
                {
                    stack.Add(models[i].GetType(), models[i]);
                }
            }
            return (T)stack[typeof(T)];
        }
    }
}
