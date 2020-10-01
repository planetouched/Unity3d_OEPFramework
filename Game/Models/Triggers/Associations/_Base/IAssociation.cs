using Basement.BLFramework.Core.Model;

namespace Game.Models.Triggers.Associations._Base
{
    public interface IAssociation : IModel
    {
        AssociationCategories categories { get; }
        void Activate();
        void Connect();
        void Disconnect();
    }
}