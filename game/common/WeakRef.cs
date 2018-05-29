using System;

namespace Assets.game.common
{
    public class WeakRef<T> where T : class
    {
        private readonly WeakReference reference;
        public T obj { get { return isAlive ? (T)reference.Target : null; } }
        public bool isAlive { get { return reference != null && reference.IsAlive; } }
        public bool trackResurrection { get { return reference.TrackResurrection; } }

        public WeakRef(T reference)
        {
            this.reference = new WeakReference(reference);
        }
    }
}
