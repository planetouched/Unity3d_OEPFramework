using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif

namespace Assets.OEPFramework.unityEngine.loop
{
    public static class EngineLoopManager
    {
        private static readonly List<EngineLoop> loops = new List<EngineLoop>();

        public static int LoopsCount()
        {
            return loops.Count;
        }

        static bool CheckInit()
        {
#if UNITY_EDITOR
            if (loops.Count == 0)
            {
                Debug.LogError("Loops is not adjusted. Use Loops.Setup()");
                EditorApplication.isPlaying = false;
                return false;
            }
#endif
            return true;
        }


        public static EngineLoop GetEngineLoop(int loopType)
        {
            return loops[loopType];
        }

        public static int AddNewEngineLoop()
        {
            int loop = loops.Count;
            loops.Add(new EngineLoop(loop));
            return loop;
        }

        public static void Execute(int loopType)
        {
            if (!CheckInit()) return;
            loops[loopType].CallAllBehavioursActions();
        }
    }
}
