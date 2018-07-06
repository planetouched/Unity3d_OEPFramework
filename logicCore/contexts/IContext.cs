namespace Assets.logicCore.contexts
{
    public interface IContext
    {
        T GetChild<T>(string key);
    }
}
