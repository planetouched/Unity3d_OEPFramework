using System;
using System.Collections.Generic;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Core.Util;
using Basement.Common;

namespace Basement.BLFramework.Core.Model
{
    public abstract class InstanceCollection<TModel, TCategories, TDescription> : ReferenceModelBase<TCategories, TDescription>
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
                var model = Factory(collectionNode.GetNode(pair.Key));
                model.key = pair.Key;
                model.Initialization();
                base.AddChild(pair.Key, model);
            }
        }

        public override void AddChild(string collectionKey, IModel child)
        {
            _lastId = Math.Max(_lastId, int.Parse(collectionKey));
            child.key = _lastId.ToString();
            base.AddChild(_lastId.ToString(), child);
        }

        public void AddChild(TModel model)
        {
            _lastId++;
            model.key = _lastId.ToString();
            base.AddChild(_lastId.ToString(), model);
        }

        public TModel AddChild(RawNode modelNode)
        {
            _lastId++;
            var model = Factory(modelNode);
            model.key = _lastId.ToString();
            model.Initialization();
            base.AddChild(_lastId.ToString(), model);
            return model;
        }

        public IEnumerable<KeyValuePair<string, TModel>> GetCollection()
        {
            foreach (var pair in this)
            {
                yield return new KeyValuePair<string, TModel>(pair.Key, (TModel)pair.Value);
            }
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
    }
}