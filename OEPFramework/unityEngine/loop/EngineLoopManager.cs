﻿using System.Collections.Generic;
using common.logger;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OEPFramework.unityEngine.loop
{
    public static class EngineLoopManager
    {
        private static readonly List<EngineLoop> _loops = new List<EngineLoop>();

        public static int LoopsCount()
        {
            return _loops.Count;
        }

        static bool CheckInit()
        {
#if UNITY_EDITOR
            if (_loops.Count == 0)
            {
                Deb.LogError("Loops is not adjusted. Use Loops.Setup()");
                EditorApplication.isPlaying = false;
                return false;
            }
#endif
            return true;
        }


        public static EngineLoop GetEngineLoop(int loopType)
        {
            return _loops[loopType];
        }

        public static int AddNewEngineLoop()
        {
            int loop = _loops.Count;
            _loops.Add(new EngineLoop(loop));
            return loop;
        }

        public static void Execute(int loopType)
        {
            if (!CheckInit()) return;
            _loops[loopType].CallAllBehavioursActions();
        }
    }
}
