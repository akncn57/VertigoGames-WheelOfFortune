using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rewards;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerData
    {
        public int CurrentZoneIndex;
        
        // Expose as ReadOnly to prevent direct list modification from outside
        public IReadOnlyList<RewardData> CollectedRewards => _collectedRewards;
        
        [NonSerialized] public Action<int> OnZoneIndexChanged;
        [NonSerialized] public Action<RewardType, int> OnRewardChanged;
        
        [SerializeField] private List<RewardData> _collectedRewards = new();
        private static string SavePath => Path.Combine(Application.persistentDataPath, "player_data.json");
        
        /// <summary>
        /// Saves the current player progress to a JSON file.
        /// </summary>
        public void Save()
        {
            try 
            {
                var json = JsonUtility.ToJson(this, true);
                File.WriteAllText(SavePath, json);
            }
            catch (Exception e)
            {
                Logger.Error("PlayerData", $"Save failed: {e.Message}");
            }
        }

        /// <summary>
        /// Loads player data from disk or creates a new profile if not found.
        /// </summary>
        public static PlayerData Load()
        {
            if (File.Exists(SavePath))
            {
                try 
                {
                    var json = File.ReadAllText(SavePath);
                    return JsonUtility.FromJson<PlayerData>(json);
                }
                catch (Exception e)
                {
                    Logger.Error("PlayerData", $"Load failed: {e.Message}");
                }
            }
            
            return new PlayerData();
        }

        /// <summary>
        /// Updates the current zone and saves the progress.
        /// </summary>
        public void SetZoneIndex(int zoneIndex)
        {
            if (zoneIndex < 0)
            {
                Logger.Error("PlayerData", "ZoneIndex out of range!");
                return;
            }
            
            CurrentZoneIndex = zoneIndex;
            OnZoneIndexChanged?.Invoke(CurrentZoneIndex);
            Save();
        }
        
        /// <summary>
        /// Adds a reward to the collection and notifies listeners.
        /// </summary>
        public void AddReward(RewardData reward)
        {
            if (reward.RewardCount < 0)
            {
                Logger.Error("PlayerData", "RewardCount cannot be negative!");
                return;
            }
            
            // Check if we already have this reward type in our list
            var existingReward = _collectedRewards.FirstOrDefault(r => r.RewardType == reward.RewardType);

            if (existingReward != null)
            {
                // Update quantity if already exists
                existingReward.RewardCount += reward.RewardCount;
            }
            else
            {
                // Create a new entry if it's a new reward type
                _collectedRewards.Add(new RewardData
                {
                    RewardType = reward.RewardType,
                    RewardCount = reward.RewardCount,
                });
            }
            
            OnRewardChanged?.Invoke(reward.RewardType, GetRewardAmount(reward.RewardType));
            Save();
        }
        
        /// <summary>
        /// Tries to spend a certain amount of rewards (e.g., for Revive).
        /// Returns true if successful.
        /// </summary>
        public bool TrySpendReward(RewardType type, int amount)
        {
            if (amount <= 0)
            {
                Logger.Error("PlayerData", "Spend amount must be positive!");
                return false;
            }
            
            var existingReward = _collectedRewards.FirstOrDefault(r => r.RewardType == type);

            // Check if player has enough balance
            if (existingReward == null || existingReward.RewardCount < amount)
            {
                return false;
            }

            existingReward.RewardCount -= amount;

            OnRewardChanged?.Invoke(type, existingReward.RewardCount);
            Save();
            return true;
        }
        
        /// <summary>
        /// Returns the total count of a specific reward type.
        /// </summary>
        public int GetRewardAmount(RewardType type)
        {
            var reward = _collectedRewards.FirstOrDefault(r => r.RewardType == type);
            
            // Return 0 if reward is not found (null-coalescing)
            return reward?.RewardCount ?? 0;
        }
    }
}