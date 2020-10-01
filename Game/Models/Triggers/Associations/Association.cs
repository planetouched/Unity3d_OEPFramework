using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Util;
using Basement.Common;
using Game.Models.Triggers.Associations._Base;

namespace Game.Models.Triggers.Associations
{
    public class Association : ModelBase, IAssociation
    {
        protected RawNode initNode;
        protected RawNode descriptionNode;
        public AssociationCategories categories { get; }
        
        protected Association(RawNode initNode, RawNode descriptionNode, AssociationCategories categories, IContext context) : base("assoc", context)
        {
            this.initNode = initNode;
            this.descriptionNode = descriptionNode;
            this.categories = categories;
        }

        public virtual void Activate() {}
        public virtual void Connect() {}
        public virtual void Disconnect() {}

        protected void Complete()
        {
            Call(categories.complete);
        }
    }
}