using System;

namespace Assets.common.random
{
    public class LRandom
    {
        private const int a = 16807;
        private const int m = 2147483647;
        private const int q = 127773;
        private const int r = 2836;
        private int seed;

        public LRandom(int seed)
        {
            if (seed <= 0 || seed == int.MaxValue)
                throw new Exception("Bad seed");
            this.seed = seed;
        }

        public double Next(bool moveSeed = true)
        {
            int hi = seed / q;
            int lo = seed % q;
            seed = a * lo - r * hi;

            if (seed <= 0 && moveSeed)
                seed = seed + m;

            return seed * 1.0 / m;
        }

        public int NextInt(int inclusiveMin, int exclusiveMax, bool moveSeed = true)
        {
            var result = Next(moveSeed);
            return (int)((exclusiveMax - inclusiveMin) * result + inclusiveMin);
        }
    }
}
