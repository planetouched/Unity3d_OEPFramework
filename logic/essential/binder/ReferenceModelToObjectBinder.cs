using System;
using System.Collections.Generic;
using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.model;
using Assets.logic.essential.path;

namespace Assets.logic.essential.binder
{
    public class ReferenceModelToObjectBinder
    {
        internal class Binder
        {
            private readonly WeakRef<IContext> weakContext;
            private readonly Dictionary<string, object> descriptions = new Dictionary<string, object>();
            private readonly string contextNamePrefix;
            private readonly Func<RawNode, object> factory;

            public Binder(WeakRef<IContext> context, Func<RawNode, object> factory, string contextNamePrefix = "!")
            {
                this.factory = factory;
                this.contextNamePrefix = contextNamePrefix;
                weakContext = context;
            }

            public object GetDescription(IReferenceModel model)
            {
                var path = PathUtil.StringPath(model);

                object description;
                if (descriptions.TryGetValue(path, out description))
                {
                    return description;
                }

                var node = PathUtil.RawNodePath(weakContext.obj, contextNamePrefix + path, null);
                description = factory(node);
                descriptions.Add(path, description);
                return description;
            }
        }

        private readonly Dictionary<Type, Binder> binders = new Dictionary<Type, Binder>();
        private readonly WeakRef<IContext> weakContext;

        public ReferenceModelToObjectBinder(IContext context)
        {
            weakContext = new WeakRef<IContext>(context);
        }

        public void AddBinder<TModel>(Func<RawNode, object> factory, string contextNamePrefix = "!") where TModel : IReferenceModel
        {
            binders.Add(typeof(TModel), new Binder(weakContext, factory, contextNamePrefix));
        }

        public TDescription GetDescription<TDescription>(IReferenceModel model) where TDescription : class
        {
            return (TDescription)binders[model.GetType()].GetDescription(model);
        }
    }
}
