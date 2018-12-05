using System;

namespace Assets.logic.essential.time
{
    public static class TimeUtil
    {
        private static readonly DateTime shift = new DateTime(1970, 1, 1);
        private static long deltaTime;

        public static void SetDelta(long unixTimestamp)
        {
            deltaTime = unixTimestamp - GetUnixTime();
        }

        public static long GetUnixTime(long unixTimestamp = 0)
        {
            return unixTimestamp == 0 ? (long)(DateTime.UtcNow - shift).TotalSeconds + deltaTime : unixTimestamp;
        }
    }
}
