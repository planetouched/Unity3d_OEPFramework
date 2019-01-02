using Assets.OEPFramework.futures;

namespace Assets.game.assetBundle.futures
{
    public class CascadeLoadingPromise : Future
    {
        private readonly CascadeLoading cascade;
        public CascadeLoadingPromise(CascadeLoading cascade)
        {
            SetAsPromise();
            this.cascade = cascade;
        }
        protected override void OnRun()
        {
            cascade.onComplete = Complete;
            cascade.Run();
        }

        protected override void OnComplete()
        {
            cascade.onComplete = null;
        }
    }
}
