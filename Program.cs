using System;
using Assets.common.random;

namespace OEPFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new LRandom(1);

            int[] r = new int[100];

            for (int i = 0; i < 1000; i++)
            {
                int result = rnd.NextInt(0, 100);
                r[result]++;
            }

            for (var i = 0; i < r.Length; i++)
            {
                var v = r[i];
                Console.WriteLine(i +" : " + v);
            }

            Console.ReadKey();
        }
    }
}
