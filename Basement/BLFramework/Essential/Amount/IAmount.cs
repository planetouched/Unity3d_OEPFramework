namespace Basement.BLFramework.Essential.Amount
{
    public interface IAmount
    {
        string type { get; }
        int Number();
    }
}
