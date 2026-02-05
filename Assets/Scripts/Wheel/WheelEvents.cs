using System;
using Rewards;

namespace Wheel
{
    public static class WheelEvents
    {
        public static event Action OnSpinStarted;
        public static event Action OnSpinEnded;
        public static event Action<int> OnZoneChanged;
        public static event Action<RewardData> OnRewardGiven;
    }
}