using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rewards;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class PlayerData
    {
        public int CurrentZoneIndex = 0;
        public List<RewardData> CollectedRewards = new();
        
        public Action<int> OnZoneIndexChanged;
        public Action<RewardType, int> OnRewardChanged;
        
        private static string SavePath => Path.Combine(Application.persistentDataPath, "player_data.json");
        
        public void Save()
        {
            var json = JsonUtility.ToJson(this, true);
            File.WriteAllText(SavePath, json);
        }

        public static PlayerData Load()
        {
            if (File.Exists(SavePath))
            {
                var json = File.ReadAllText(SavePath);
                return JsonUtility.FromJson<PlayerData>(json);
            }

            return new PlayerData();
        }

        public void SetZoneIndex(int zoneIndex)
        {
            CurrentZoneIndex = zoneIndex;
            OnZoneIndexChanged?.Invoke(CurrentZoneIndex);
        }
        
        public void AddReward(RewardData reward)
        {
            var existingReward = CollectedRewards.FirstOrDefault(r => r.RewardType == reward.RewardType);

            if (existingReward != null)
            {
                existingReward.RewardCount += reward.RewardCount;
            }
            else
            {
                CollectedRewards.Add(new RewardData
                {
                    RewardType = reward.RewardType,
                    RewardCount = reward.RewardCount,
                });
            }
            
            OnRewardChanged?.Invoke(reward.RewardType, GetRewardAmount(reward.RewardType));
            Save();
        }
        
        public bool TrySpendReward(RewardType type, int amount)
        {
            var existingReward = CollectedRewards.FirstOrDefault(r => r.RewardType == type);

            if (existingReward == null || existingReward.RewardCount < amount)
            {
                return false;
            }

            existingReward.RewardCount -= amount;

            OnRewardChanged?.Invoke(type, existingReward.RewardCount);
            Save();
            return true;
        }
        
        public int GetRewardAmount(RewardType type)
        {
            var reward = CollectedRewards.FirstOrDefault(r => r.RewardType == type);
            return reward?.RewardCount ?? 0;
        }
    }
}