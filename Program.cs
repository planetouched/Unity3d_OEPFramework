﻿using System;
using System.IO;
using Basement.BLFramework.Core.Factories;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.BLFramework.Core.Util;
using Basement.BLFramework.Essential.Amounts;
using Basement.BLFramework.Essential.Choices;
using Basement.BLFramework.Essential.EssentialRandom.Implementation;
using Basement.BLFramework.Essential.Prices;
using Basement.BLFramework.Essential.Requirements;
using Basement.BLFramework.Essential.Rewards;
using Basement.BLFramework.Essential.Rewards.Result;
using Basement.Common;
using Test;
using Test.Buildings;
using Test.Cities;
using Test.Simple;
using fastJSON;
using Game.Models.GameResources.Simple;

namespace Basement454545
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            int[] r = new int[20];
            for (int j = 0; j < 10; j++)
            {
                var rnd = new FastRandom2(j);
                for (int i = 0; i < 100000000; i++)
                {
                    int result = rnd.Range(1, 20);
                    r[result]++;
                }
            }

            for (int i = 0; i < r.Length; i++)
            {
                Console.WriteLine(i + " -> " + r[i]);
            }*/

            FactoryManager.SetDefaultFactory(new DefaultFactory());

            FactoryManager.AddFactory(typeof(IRandomImplementation), new Factory())
                .AddVariant("fast", typeof(FastRandom));

            FactoryManager.AddFactory(typeof(Price), new Factory())
                .AddVariant("simple-resource", typeof(SimpleResourcePrice))

                .AddVariant("simple", typeof(SimpleResourcePrice))
                .AddVariant("composite", typeof(CompositeReward));

            FactoryManager.AddFactory(typeof(PathChoice), new Factory())
                .AddVariant("rle", typeof(RleSetPathChoice))
                .AddVariant("simple", typeof(SimplePathChoice));

            FactoryManager.AddFactory(typeof(Reward), new Factory())
                .AddVariant("simple-resource", typeof(SimpleResourceReward))
                .AddVariant("composite", typeof(CompositeReward))
                .AddVariant("random", typeof(RandomReward));

            FactoryManager.AddFactory(typeof(RewardResult), new Factory())
                .AddVariant("simple-resource", typeof(SimpleResourceRewardResult))
                .AddVariant("composite", typeof(CompositeRewardResult));

            FactoryManager.AddFactory(typeof(Requirement), new Factory())
                .AddVariant("simple-resource", typeof(SimpleResourceRequirement))
                .AddVariant("and", typeof(AndRequirement))
                .AddVariant("not", typeof(NotRequirement))
                .AddVariant("or", typeof(OrRequirement))
                .AddVariant("composite", typeof(CompositeRequirement))
                .AddVariant("false", typeof(FalseRequirement))
                .AddVariant("city", typeof(CityRequirement))
                .AddVariant("building", typeof(BuildingRequirement));

            FactoryManager.AddFactory(typeof(Amount), new Factory())
                .AddVariant("simple", typeof(SimpleAmount))
                .AddVariant("set", typeof(SetAmount))
                .AddVariant("rle", typeof(RleSetAmount))
                .AddVariant("range", typeof(RangeAmount))
                .AddVariant("critical", typeof(CriticalAmount));


            var mockNode = new RawNode(JSON.Instance.Parse(File.ReadAllText("mock.json")));

            var simpleResourceNode = JSON.Instance.Parse(File.ReadAllText("simple-resources.json"));
            var randomNode = JSON.Instance.Parse(File.ReadAllText("random.json"));
            var objectsNode = JSON.Instance.Parse(File.ReadAllText("objects.json"));
            var dealsNode = JSON.Instance.Parse(File.ReadAllText("deals.json"));

            var citiesNode = JSON.Instance.Parse(File.ReadAllText("cities.json"));

            var dict = SerializeUtil.Dict().SetArgs("simple-resources", simpleResourceNode, "objects", objectsNode, "random",
                randomNode, "deals", dealsNode, "cities", citiesNode);

            var player = new Player(mockNode, new RawNode(dict));

            var territories = new Territories("territories", new TerritoryCategory(), player);
            territories.Attach(territories.category.tank.fire, OnTerritories);
            territories["0"].Attach(territories.category.tank.fire, OnTerritory);
            territories["0"].tanks.Attach(territories.category.tank.fire, OnTanks);
            territories["0"].tanks["0"].Attach(territories.category.tank.fire, OnTank);
            territories["0"].tanks["0"].Fire();

            var cities = player.GetChild<CityCollection>("cities");
            var model = cities["0"];

            cities.Attach(cities.categories.buildings.complete, Handler);
            model.buildings["0"].Complete();

            Console.ReadKey();
        }

        private static void Handler(CoreParams cp, object args)
        {
        }

        private static void OnSR(CoreParams cp, object args)
        {
            Console.WriteLine("OnSR");
        }

        private static void OnSRS(CoreParams cp, object args)
        {
            Console.WriteLine("OnSRS");
        }

        private static void OnTank(CoreParams cp, object args)
        {
            Console.WriteLine("tank");
        }

        private static void OnTanks(CoreParams cp, object args)
        {
            Console.WriteLine("tanks");
        }

        private static void OnTerritory(CoreParams cp, object args)
        {
            Console.WriteLine("territory");
        }

        private static void OnTerritories(CoreParams cp, object args)
        {
            Console.WriteLine("territories");
        }
    }
}
