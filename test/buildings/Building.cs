using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Util;
using Basement.Common;

namespace Test.Buildings
{
    public class Building : ReferenceModelBase<BuildingCategories, BuildingDescription>
    {
        public bool completed { get; private set; }

        public Building(RawNode initNode, BuildingCategories categories, BuildingDescription description, IContext context) : base(initNode, categories, description, context)
        {
            completed = initNode.GetBool("completed");
        }

        public void Complete()
        {
            completed = true;
            Call(categories.complete, null);
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("completed", completed);
        }
    }
}
