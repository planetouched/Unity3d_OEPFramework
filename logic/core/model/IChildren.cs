namespace Assets.logic.core.model
{
    public interface IChildren
    {
        IModel GetChild(string key);
        void AddChild(string key, IModel model);
        void RemoveChild(string key);
        bool Exist(string key);
        int Count();
    }
}
