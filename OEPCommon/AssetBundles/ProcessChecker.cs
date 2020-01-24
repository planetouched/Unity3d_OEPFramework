using System;
using System.Collections.Generic;
using Basement.OEPFramework.Futures;
using OEPCommon.AssetBundles.Futures;

namespace OEPCommon.AssetBundles
{
    public class ProcessChecker : IProcess
    {
        public float loadingProgress => CalcLoadingProgress();
        public event Action<IProcess> onProcessComplete;
        public bool isComplete { get; private set; }

        private readonly List<IProcess> _processes = new List<IProcess>();
        private readonly IList<IFuture> _listToLoad = new List<IFuture>();

        private readonly IList<(string resource, bool async)> _itemsToLoad = new List<(string resource, bool async)>();
        private readonly IList<IProcess> _processesToLoad = new List<IProcess>();

        public void Add(IProcess process)
        {
            _processesToLoad.Add(process);
        }

        public void Add(string resource, bool async = true)
        {
            _itemsToLoad.Add((resource, async));
        }

        public void Load(Unloader unloader = null, int simultaneousLimit = int.MaxValue)
        {
            isComplete = false;

            foreach (var pair in _itemsToLoad)
            {
                var loadFuture = AssetBundleManager.Load(pair.resource, out var processList, pair.async);
                unloader?.Add(pair.resource);

                if (!loadFuture.isDone)
                {
                    _processes.AddRange(processList);
                    AttachFuture(loadFuture);
                }
            }

            foreach (var process in _processesToLoad)
            {
                if (!process.isComplete)
                {
                    _processes.Add(process);
                    AttachFuture(new ProcessFuture(process));
                }
            }

            _itemsToLoad.Clear();
            _processesToLoad.Clear();

            if (_listToLoad.Count == 0)
            {
                isComplete = true;
                onProcessComplete?.Invoke(this);
            }
            else
            {
                for (int i = 0; i < simultaneousLimit; i++)
                {
                    if (i < _listToLoad.Count)
                    {
                        var future = _listToLoad[i];
                        future.Run();

                        if (future.isDone)
                        {
                            i--;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        
        private void AttachFuture(IFuture future)
        {
            _listToLoad.Add(future);
            future.AddListener(Loading_Complete);
        }
        
        private void Loading_Complete(IFuture completedFuture)
        {
            _listToLoad.Remove(completedFuture);
            
            if (_listToLoad.Count == 0)
            {
                _processes.Clear();
                isComplete = true;
                onProcessComplete?.Invoke(this);
            }
            else
            {
                foreach (var item in _listToLoad)
                {
                    if (!item.wasRun)
                    {
                        item.Run();
                        break;
                    }
                }
            }
        }

        private float CalcLoadingProgress()
        {
            if (isComplete)
            {
                return 1;
            }

            if (_processes.Count == 0)
            {
                return 0;
            }

            float value = 0;

            for (var i = 0; i < _processes.Count; i++)
            {
                var progress = _processes[i];
                value += progress.loadingProgress;
            }

            return value / _processes.Count;
        }

        public void Cancel()
        {
            _itemsToLoad.Clear();
            _processesToLoad.Clear();

            foreach (var item in _listToLoad)
            {
                item.RemoveListener(Loading_Complete);
            }
            
            _listToLoad.Clear();

            foreach (var process in _processes)
            {
                process.Cancel();
            }
            
            _processes.Clear();
            
            onProcessComplete = null;
        }
    }
}