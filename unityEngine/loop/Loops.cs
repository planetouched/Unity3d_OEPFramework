namespace OEPFramework.unityEngine.loop
{
    public static class Loops
    {
        public static int TIMER { get; private set; }
        public static int FIXED_UPDATE { get; private set; }
        public static int UPDATE { get; private set; }
        public static int LATE_UPDATE { get; private set; }
        public static int LEGACY_GUI { get; private set; }

        public static void Setup()
        {
            TIMER = EngineLoopManager.AddNewEngineLoop();
            FIXED_UPDATE = EngineLoopManager.AddNewEngineLoop();
            UPDATE = EngineLoopManager.AddNewEngineLoop();
            LATE_UPDATE = EngineLoopManager.AddNewEngineLoop();
            LEGACY_GUI = EngineLoopManager.AddNewEngineLoop();
        }
    }
}
