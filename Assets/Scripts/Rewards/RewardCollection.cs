using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rewards
{
    [CreateAssetMenu(fileName = "RewardCollection", menuName = "Rewards/Collection")]
    public class RewardCollection : ScriptableObject
    {
        public List<RewardItemSO> allRewards = new();
        
        public RewardItemSO GetRewardByType(RewardType type)
        {
            var reward = allRewards.FirstOrDefault(r => r != null && r.type == type);

            if (reward == null)
            {
                Debug.LogError($"[RewardCollection] Reward of type {type} was not found in the database!");
            }

            return reward;
        }
    }
}