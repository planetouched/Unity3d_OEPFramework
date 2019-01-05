using System;
using System.Collections.Generic;
using common;
using logic.core.context;
using logic.core.reference.description;
using logic.core.util;

namespace logic.core.model
{
    public abstract class
        InstanceCollection<TModel, TCategories, TDescription> : ReferenceModelBase<TCategories, TDescription>
        where TModel : IModel
        where TDescription : IDescription
        where TCategories : class
    {
        private int _lastId;

        protected InstanceCollection(RawNode initNode, TCategories categories, TDescription description,
            IContext context) : base(initNode, categories, description, context)
        {
        }

        public TModel this[string collectionKey] => (TModel) GetChild(collectionKey);

        public override void Initialization()
        {
            base.Initialization();

            var collectionNode = initNode.GetNode("collection");

            foreach (var pair in collectionNode.GetUnsortedCollection())
            {
                _lastId = Math.Max(_lastId, int.Parse(pair.Key));
                base.AddChild(pair.Key, Factory(collectionNode.GetNode(pair.Key)));
            }
        }

        public override void AddChild(string collectionKey, IModel model)
        {
            _lastId = Math.Max(_lastId, int.Parse(collectionKey));
            model.key = _lastId.ToString();
            model.onDestroy += OnDestroy;
            base.AddChild(_lastId.ToString(), model);
        }

        public void AddChild(TModel model)
        {
            _lastId++;
            model.key = _lastId.ToString();
            model.onDestroy += OnDestroy;
            base.AddChild(_lastId.ToString(), model);
        }

        public TModel AddChild(RawNode modelNode)
        {
            _lastId++;
            var model = Factory(modelNode);
            model.key = _lastId.ToString();
            model.onDestroy += OnDestroy;
            base.AddChild(_lastId.ToString(), model);
            return model;
        }

        private void OnDestroy(IModel model)
        {
            RemoveChild(model.key);
        }

        protected abstract TModel Factory(RawNode modelInitNode);

        public override object Serialize()
        {
            var dict = SerializeUtil.Dict();

            foreach (var pair in this)
            {
                dict.Add(pair.Key, pair.Value.Serialize());
            }

            return SerializeUtil.Dict().SetArgs("collection", dict);
        }

        public override void Destroy()
        {
            var copy = new List<KeyValuePair<string, IModel>>(this);

            foreach (var pair in copy)
            {
                pair.Value.Destroy();
            }

            base.Destroy();
        }
    }
}