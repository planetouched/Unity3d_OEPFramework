using System.Collections.Generic;

namespace logic.core.model
{
    public interface IReferenceCollection : IModel, IChildren
    {
        IEnumerable<string> GetUnsortedKeys();
        IEnumerable<string> GetSortedKeys();
    }
}