using System;

namespace Assets.common
{
    public class Lazy<T>
    {
        private T value;
        private bool created;

        public bool isCreated { get { return created; } }
        private Func<T> initFunc;
        public Action<T> onCreate  { get; set; }

        public Lazy(Func<T> initFunc)
        {
            this.initFunc = initFunc;
        }

        public void Create()
        {
            GetValue();
        }

        public T GetValue()
        {
            if (created)
                return value;

            value = initFunc();
            initFunc = null;
            created = true;
            
            if (onCreate != null)
                onCreate(value);

            return value;
        }

        public void ClearFactory()
        {
            initFunc = null;
        }
    }
}
