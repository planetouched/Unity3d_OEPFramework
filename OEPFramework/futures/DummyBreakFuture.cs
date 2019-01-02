namespace Assets.OEPFramework.futures
{
    public class DummyBreakFuture : Future
    {

        protected override void OnRun()
        {
            Complete();
        }

        protected override void OnComplete()
        {
        }
    }
}
