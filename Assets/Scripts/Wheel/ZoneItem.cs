using System;
using System.Collections.Generic;
using Rewards;

namespace Wheel
{
    [Serializable]
    public class ZoneItem
    {
        public List<RewardData> ZoneRewards = new List<RewardData>();
        public ZoneType ZoneType;
    }
}