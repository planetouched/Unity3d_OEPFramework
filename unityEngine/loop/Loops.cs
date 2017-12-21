namespace OEPFramework.unityEngine.loop
{
    public static partial class Loops
    {
        public static int TIMER = EngineLoopManager.AddNewEngineLoop();
        public static int FIXED_UPDATE = EngineLoopManager.AddNewEngineLoop();
        public static int UPDATE = EngineLoopManager.AddNewEngineLoop();
        public static int LATE_UPDATE = EngineLoopManager.AddNewEngineLoop();
        public static int LEGACY_GUI = EngineLoopManager.AddNewEngineLoop();
    }
}
