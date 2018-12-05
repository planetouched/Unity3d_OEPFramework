using Assets.logic.core.model;

namespace Assets.logic.core.context
{
    public interface IContext : IChildren
    {
        T GetChild<T>(string key);
    }
}
