using System;
using System.Collections.Generic;

namespace Assets.game.common.pool
{
    public class ObjectPool<T> : IObjectPool where T : class
    {
        readonly Dictionary<T, bool> checker = new Dictionary<T, bool>();
        readonly Queue<T> queue = new Queue<T>();
        readonly object locker = new object();

        public int unusedObjectsCount
        {
            get
            {
                lock (locker)
                    return queue.Count;
            }
        }

        public int usedObjectsCount
        {
            get
            {
                lock (locker)
                    return checker.Count - queue.Count;
            }
        }

        public int objectsCount
        {
            get
            {
                lock (locker)
                    return checker.Count;
            }
        }

        private Func<T> createFunc;

        public ObjectPool(Func<T> createFunc = null)
        {
            this.createFunc = createFunc;
        }

        public void SetFactory(Func<T> factory)
        {
            createFunc = factory;
        }

        public void UnsetFactory()
        {
            createFunc = null;
        }

        public void Add(T obj)
        {
            lock (locker)
            {
                if (checker.ContainsKey(obj))
                    throw new Exception("ObjectPool::Add - это объект уже был добавлен в ObjectPool");
                
                checker.Add(obj, true);
                queue.Enqueue(obj);
            }
        }

        public void ReturnObj(object obj)
        {
            Return((T)obj);
        }
        
        public void Return(T obj)
        {
            lock (locker)
            {
                if (checker[obj])
                    throw new Exception("ObjectPool::Return - попытка вернуть объект дважды");

                checker[obj] = true;
                queue.Enqueue(obj);
            }
        }

        public T Take()
        {
            lock (locker)
            {
                T obj;

                if (queue.Count == 0)
                {
                    if (createFunc == null)
                        return null;

                    obj = createFunc();
                    checker.Add(obj, false);
                }
                else
                {
                    obj = queue.Dequeue();
                    checker[obj] = false;
                }

                return obj;
            }
        }

        public bool ForEach(Action<T> iterationAction)
        {
            if (usedObjectsCount > 0)
                return false;

            foreach (var item in queue)
                iterationAction(item);

            return true;
        }

        public bool RemoveAll(Action<T> destroyAction = null)
        {
            if (usedObjectsCount > 0)
                return false;

            if (destroyAction != null)
            {
                foreach (var item in queue)
                    destroyAction(item);
            }

            queue.Clear();
            checker.Clear();
            return true;
        }
    }
}