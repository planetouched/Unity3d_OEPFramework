namespace Assets.common.random
{
    public class FastRandom
    {
        const double RealUnitInt = 1.0 / (int.MaxValue + 1.0);
        const int W = 273326509;

        public int seed { get; private set; }

        public FastRandom(int seed)
        {
            this.seed = seed;
        }

        public double Next(bool moveSeed = true)
        {
            int x = ((seed * 1431655781) & 0x7fffffff)
                     + ((seed * 1183186591) & 0x7fffffff)
                     + ((seed * 622729787) & 0x7fffffff)
                     + ((seed * 338294347) & 0x7fffffff) & 0x7fffffff;

            if (moveSeed)
                seed++;

            int t = (x ^ (x << 11)) & 0x7fffffff;
            return RealUnitInt * (0x7FFFFFFF & ((W ^ (W >> 19)) ^ (t ^ (t >> 8))));
        }

        public int NextInt(int inclusiveMin, int exclusiveMax, bool moveSeed = true)
        {
            var result = Next(moveSeed);
            return (int)((exclusiveMax - inclusiveMin) * result + inclusiveMin);
        }
    }
}
