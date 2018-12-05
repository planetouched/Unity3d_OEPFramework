﻿
namespace Assets.logic.essential.random
{
    public interface IRandom
    {
        int GetSeed();
        double NextDouble(bool incSeed = true);
        int NextInt(bool incSeed = true);
        int Range(int inclusiveMin, int exclusiveMax, bool incSeed = true);
    }
}