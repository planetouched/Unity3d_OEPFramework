using System;
using System.Collections.Generic;
using Assets.game.assetBundle.future;
using Assets.game.common.utils;
using Assets.OEPFramework.future;
using Assets.OEPFramework.future.utils.threadsafe;

namespace Assets.game.assetBundle
{
    public class ProcessChecker : IProcess
    {
        private readonly AssetBundleManager manager;

        public float loadingProgress
        {
            get { return CalcLoadingProgress(); }
        }

        public float unpackProgress
        {
            get { return CalcUnpackProgress(); }
        }

        public Action<IProcess> onProcessComplete { get; set; }
        public bool isComplete { get; private set; }

        private readonly FutureWatcher watcher = new FutureWatcher();
        readonly List<IProcess> processes = new List<IProcess>();
        private bool loading;

        public ProcessChecker()
        {
            manager = SingletonManager.Get<AssetBundleManager>();
        }

        public void Add(IProcess process)
        {
            processes.Add(process);
            watcher.AddFuture(new ProcessFuture(process).Run());
        }

        public void Add(string resource, Unloader unloader = null, bool async = true)
        {
            isComplete = false;

            if (unloader != null)
                unloader.Add(resource);

            List<IProcess> processList;
            var compositeFuture = manager.Load(resource, out processList, async);
            foreach (var loadFuture in processList)
            {
                processes.Add(loadFuture);
            }

            watcher.AddFuture(compositeFuture);

            if (loading)
                Subscribe(compositeFuture);
        }

        void Subscribe(IFuture target)
        {
            target.AddListener(future =>
            {
                if (watcher.futuresCount == 0 && future.isDone)
                {
                    processes.Clear();
                    isComplete = true;
                    loading = false;

                    if (onProcessComplete != null)
                        onProcessComplete(this);
                }
            });
        }

        public void Load()
        {
            if (CallCompleteIfEmpty() || loading)
                return;

            loading = true;

            foreach (var future in watcher.futures)
            {
                Subscribe(future);
            }
        }

        bool CallCompleteIfEmpty()
        {
            if (watcher.futuresCount == 0)
            {
                isComplete = true;
                if (onProcessComplete != null)
                    onProcessComplete(this);

                return true;
            }
            return false;
        }

        float CalcUnpackProgress()
        {
            if (isComplete) return 1;
            if (processes.Count == 0)
                return 0;

            float value = 0;
            foreach (var progress in processes)
                value += progress.unpackProgress;
            
            return value / processes.Count;
        }
        
        float CalcLoadingProgress()
        {
            if (isComplete) return 1;
            if (processes.Count == 0)
                return 0;

            float value = 0;
            foreach (var progress in processes)
                value += progress.loadingProgress;

            return value / processes.Count;
        }
        
        public void Drop()
        {
            processes.Clear();
            watcher.CancelFutures();
        }
    }
}
