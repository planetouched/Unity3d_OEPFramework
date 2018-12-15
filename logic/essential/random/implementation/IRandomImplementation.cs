namespace logic.essential.random.implementation
{
    public interface IRandomImplementation
    {
        double NextDouble(ref int seed, bool incSeed);
        int NextInt(ref int seed, int exlusiveMax, bool incSeed);
        int Range(int inclusiveMin, int exclusiveMax, ref int seed, bool incSeed);
    }
}