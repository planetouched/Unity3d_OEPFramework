using System;

namespace common
{
    public class Lazy<T>
    {
        private T value;

        public bool isCreated { get; private set; }
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
            if (isCreated)
                return value;

            value = initFunc();
            initFunc = null;
            isCreated = true;
            
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
