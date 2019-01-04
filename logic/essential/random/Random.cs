using System.Collections.Generic;
using common;
using logic.core.context;
using logic.core.factories;
using logic.core.model;
using logic.essential.random.implementation;

namespace logic.essential.random
{
    public class Random : ReferenceModelBase<RandomCategories, RandomDescription>, IRandom
    {
        private int _seed;
        private readonly IRandomImplementation _random;

        public Random(RawNode initNode, RandomCategories categories, RandomDescription description, IContext context) : base(initNode, categories, description, context)
        {
            _seed = initNode.GetInt("seed");
            _random = (IRandomImplementation)FactoryManager.Build(typeof(IRandomImplementation), description.type);
        }

        public int GetSeed()
        {
            return _seed;
        }

        public double NextDouble(bool incSeed = true)
        {
            return _random.NextDouble(ref _seed, incSeed);
        }

        public int NextInt(int exclusiveMax = int.MaxValue, bool incSeed = true)
        {
            return _random.NextInt(ref _seed, exclusiveMax, incSeed);
        }

        public int Range(int inclusiveMin, int exclusiveMax, bool incSeed = true)
        {
            return _random.Range(inclusiveMin, exclusiveMax, ref _seed, incSeed);
        }

        public override object Serialize()
        {
            return new Dictionary<string, object> { { "seed", _seed } };
        }
    }
}
