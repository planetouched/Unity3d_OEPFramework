using System;
using System.Collections.Generic;
using common;
using game.model.resource.simple;
using logic.core.context;
using logic.core.model;
using logic.essential.random;
using test.reference.someModel;
using test.simple;

namespace test
{
    public class Player : IContext
    {
        public DataSources dataSources { get; }
        public RawNode repositoryNode { get; }

        private readonly Dictionary<string, object> children = new Dictionary<string, object>();

        public Territories territories;

        public Player(RawNode initNode, RawNode repositoryNode)
        {
            this.repositoryNode = repositoryNode;

            dataSources = new DataSources();

            var simpleResources = new SimpleResourceCollection(initNode.GetNode("simple-resources"), new SimpleResourceCategories(), this, new SimpleResourceDataSource(repositoryNode.GetNode("simple-resources"), this));
            var objects = new SomeModelCollection(initNode.GetNode("objects"), new SomeModelCategories(), this, new SomeModelDataSource(repositoryNode.GetNode("objects"), this));
            var random = new RandomCollection(initNode.GetNode("random"), new RandomCategories(), this, new RandomDataSource(repositoryNode.GetNode("random"), this));

            AddChild("random", random);
            AddChild("simple-resources", simpleResources);
            AddChild("objects", objects);
        }

        public IModel GetChild(string collectionKey)
        {
            return (IModel)children[collectionKey];
        }

        public void AddChild(string collectionKey, IModel obj)
        {
            children.Add(collectionKey, obj);
        }

        public void RemoveChild(string collectionKey, bool destroy)
        {
            throw new NotImplementedException();
        }

        public bool Exist(string collectionKey)
        {
            return children.ContainsKey(collectionKey);
        }

        public T GetChild<T>(string collectionKey) where T : class
        {
            return (T)children[collectionKey];
        }
    }
}
