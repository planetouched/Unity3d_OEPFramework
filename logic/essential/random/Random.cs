using System.Collections.Generic;
using Assets.common;
using Assets.logic.core.context;
using Assets.logic.core.factories;
using Assets.logic.core.model;
using Assets.logic.essential.random.implementation;

namespace Assets.logic.essential.random
{
    public class Random : ReferenceModelBase<RandomDescription>, IRandom
    {
        private int seed;
        private readonly IRandomImplementation random;

        public Random(RawNode initNode, RandomDescription description, IContext context) : base(initNode, description, context)
        {
            seed = initNode.GetInt("seed");
            random = (IRandomImplementation)FactoryManager.Build(typeof(IRandomImplementation), description.type);
        }

        public int GetSeed()
        {
            return seed;
        }

        public double NextDouble(bool incSeed = true)
        {
            return random.NextDouble(ref seed, incSeed);
        }

        public int NextInt(bool incSeed = true)
        {
            return random.NextInt(ref seed, incSeed);
        }

        public int Range(int inclusiveMin, int exclusiveMax, bool incSeed = true)
        {
            return random.Range(inclusiveMin, exclusiveMax, ref seed, incSeed);
        }

        public override object Serialize()
        {
            return new Dictionary<string, object> { { "seed", seed } };
        }
    }
}
