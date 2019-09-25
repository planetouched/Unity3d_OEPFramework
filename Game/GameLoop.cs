using Basement.OEPFramework.UnityEngine;
using Basement.OEPFramework.UnityEngine.Loop;
using Game.Preloader;
using UnityEngine;

namespace Game
{
    public class GameLoop : MonoBehaviour
    {
        public static string EVENT_APP_QUIT = GEvent.GetUniqueCategory();
        public static string EVENT_APP_PAUSE = GEvent.GetUniqueCategory();

        void Awake()
        {
            if (!AppStart.isInit)
            {
                Loops.Init();
                AppStart.Start();
            }
        }

        #region loop

        void OnGUI()
        {
            EngineLoopManager.Execute(Loops.LEGACY_GUI);
        }

        void FixedUpdate()
        {
            EngineLoopManager.Execute(Loops.FIXED_UPDATE);
        }

        void SimulatePhysics()
        {
            //optional
            if (!Physics2D.autoSimulation)
            {
                Physics2D.Simulate(Time.deltaTime);
            }
        }

        private void Update()
        {
            Timer.Process(Loops.TIMER);
            SimulatePhysics();
            EngineLoopManager.Execute(Loops.UPDATE);
        }

        void LateUpdate()
        {
            EngineLoopManager.Execute(Loops.LATE_UPDATE);
        }
        #endregion

        void OnApplicationPause(bool pauseStatus)
        {
            GEvent.Call(EVENT_APP_PAUSE, pauseStatus);
        }

        void OnApplicationQuit()
        {
            GEvent.Call(EVENT_APP_QUIT);
        }
    }
}