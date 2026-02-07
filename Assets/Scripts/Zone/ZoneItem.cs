using System;
using System.Collections.Generic;
using Rewards;
using Wheel;

namespace Zone
{
    [Serializable]
    public class ZoneItem
    {
        public List<RewardData> ZoneRewards = new List<RewardData>();
        public ZoneType ZoneType;
    }
}