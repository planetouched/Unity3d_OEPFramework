namespace OEPFramework.common.future
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
