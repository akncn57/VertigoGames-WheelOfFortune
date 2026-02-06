using System;
using Rewards;

namespace Wheel
{
    public static class WheelEvents
    {
        public static Action OnSpinStarted;
        public static Action<RewardData> OnSpinEnded;
        public static Action<int> OnZoneChanged;
        public static Action OnRewardGiven;
    }
}