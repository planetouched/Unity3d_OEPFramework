using Basement.OEPFramework.Futures;

namespace Game.AssetBundle.Futures
{
    public class CascadeLoadingPromise : Future
    {
        private readonly CascadeLoading _cascade;
        public CascadeLoadingPromise(CascadeLoading cascade)
        {
            SetAsPromise();
            _cascade = cascade;
        }
        protected override void OnRun()
        {
            _cascade.onComplete += Complete;
            _cascade.Run();
        }

        protected override void OnComplete()
        {
            _cascade.ClearEvent();
        }
    }
}
