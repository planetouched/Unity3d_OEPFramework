using System.Collections.Generic;

namespace Assets.logic.core.model
{
    public interface IReferenceCollection : IModel, IChildren
    {
        IEnumerable<string> GetUnsortedKeys();
        IEnumerable<string> GetSortedKeys();
    }
}