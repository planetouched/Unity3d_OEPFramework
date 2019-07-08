using Assets.test.buildings;
using common;
using logic.core.context;
using logic.core.model;
using logic.core.throughEvent;
using logic.core.util;

namespace Assets.test.cities
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
