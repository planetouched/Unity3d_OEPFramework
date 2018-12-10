using System.Collections.Generic;
using Assets.common;
using Assets.game.model.resource.simple;
using Assets.logic.core.context;
using Assets.logic.core.model;
using Assets.logic.essential.random;
using Assets.test.reference.someModel;
using Assets.test.simple;

namespace Assets.test
{
    public class Player : IContext
    {
        public RawNode repositoryNode { get; }

        private readonly Dictionary<string, object> models = new Dictionary<string, object>();

        public Territories territories;

        public Player(RawNode initNode, RawNode repositoryNode)
        {
            this.repositoryNode = repositoryNode;

            var simpleResources = new SimpleResourceCollection(initNode.GetNode("simple-resources"), new SimpleResourceCategories(), this, new SimpleResourceDataSource(repositoryNode.GetNode("simple-resources"), this));
            var objects = new SomeModelCollection(initNode.GetNode("objects"), new SomeModelCategories(), this, new SomeModelDataSource(repositoryNode.GetNode("objects"), this));
            var random = new RandomCollection(initNode.GetNode("random"), new RandomCategories(), this, new RandomDataSource(repositoryNode.GetNode("random"), this));

            AddChild("random", random);
            AddChild("simple-resources", simpleResources);
            AddChild("objects", objects);
        }

        public IModel GetChild(string collectionKey)
        {
            return (IModel)models[collectionKey];
        }

        public void AddChild(string collectionKey, IModel model)
        {
            models.Add(collectionKey, model);
        }

        public void AddChild(string collectionKey, object obj)
        {
            models.Add(collectionKey, obj);
        }

        public void RemoveChild(string collectionKey)
        {
            models.Remove(collectionKey);
        }

        public bool Exist(string collectionKey)
        {
            return models.ContainsKey(collectionKey);
        }

        public int Count()
        {
            return models.Count;
        }

        public T GetChild<T>(string collectionKey)
        {
            return (T)models[collectionKey];
        }
    }
}
