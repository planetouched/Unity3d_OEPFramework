using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.BLFramework.Core.Util;
using Basement.Common;
using Test.Buildings;

namespace Test.Cities
{
    public class City : ReferenceModelBase<CityCategories, CityDescription>
    {
        public bool completed { get; private set; }
        public BuildingCollection buildings { get; private set; }

        public City(RawNode initNode, CityCategories categories, CityDescription description, IContext context) : base(initNode, categories, description, context)
        {
            completed = initNode.GetBool("completed");
        }

        public override void Initialization()
        {
            buildings = new BuildingCollection(initNode.GetNode("buildings"), categories.buildings, GetContext(), new BuildingDataSource(description.GetNode().GetNode("buildings"), GetContext()));
            AddChild("buildings", buildings);

            buildings.Attach(categories.buildings.complete, OnBuildingComplete);
        }

        private void OnBuildingComplete(CoreParams cp, object args)
        {
            foreach (var pair in buildings)
            {
                if (!pair.Value.completed)
                {
                    return;
                }
            }

            completed = true;
        }

        public override bool CheckAvailable()
        {
            return description.requirement.Check();
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("completed", completed);
        }
    }
}
