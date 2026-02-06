using System.Collections.Generic;
using System.Linq;
using Rewards;

namespace Player
{
    public class PlayerData
    {
        public int CurrentZoneIndex = 0;
        public List<RewardData> CollectedRewards = new List<RewardData>();

        public void AddReward(RewardData reward)
        {
            var existingReward = CollectedRewards.FirstOrDefault(r => r.RewardType == reward.RewardType);

            if (existingReward != null)
            {
                existingReward.RewardCount += reward.RewardCount;
            }
            else
            {
                CollectedRewards.Add(new RewardData { 
                    RewardType = reward.RewardType, 
                    RewardCount = reward.RewardCount,
                });
            }
        }
    }
}