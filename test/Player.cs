using System;
using System.Collections.Generic;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Essential.Random;
using Basement.Common;
using Game.Models.GameResources.Simple;
using Test.Cities;
using Test.Reference.someModel;
using Test.Simple;

namespace Test
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
            var cities = new CityCollection(initNode.GetNode("cities"), new CityCategories(), this, new CityDescriptionDataSource(repositoryNode.GetNode("cities"), this));

            AddChild("random", random);
            AddChild("simple-resources", simpleResources);
            AddChild("objects", objects);
            AddChild("cities", cities);
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

        public void RemoveChild(string collectionKey)
        {
            throw new NotImplementedException();
        }
    }
}
