using Assets.OEPFramework.future;

namespace Assets.game.assetBundle.future
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
