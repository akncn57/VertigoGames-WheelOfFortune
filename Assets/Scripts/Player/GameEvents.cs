using System;
using Rewards;

namespace Wheel
{
    public static class GameEvents
    {
        public static Action<RewardData> OnWheelSpinStarted;
        public static Action<RewardData> OnWheelSpinEnded;
        public static Action<int> OnZoneChanged;
        public static Action OnInOutsStarted;
        public static Action OnInOutsEnded;
        public static Action OnRewardGiven;
        public static Action OnGameLose;
        public static Action OnReviveSuccess;
    }
}