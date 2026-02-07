using Collections;
using Rewards;

namespace Player
{
    public class PlayerData
    {
        public int CurrentZoneIndex = 0;
        public SerializedDictionary<RewardType, int> CollectedRewards = new SerializedDictionary<RewardType, int>();

        public void AddReward(RewardData reward)
        {
            if (CollectedRewards.ContainsKey(reward.RewardType))
            {
                CollectedRewards[reward.RewardType] += reward.RewardCount;
            }
            else
            {
                CollectedRewards.Add(reward.RewardType, reward.RewardCount);
            }
        }
    }
}