using UnityEngine;
using Wheel;
using Rewards;

namespace Player
{
    /// <summary>
    /// Core manager that coordinates game logic, player data, and events.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Configurations")]
        [SerializeField] private ZoneDataSO zoneData;
        [SerializeField] private RewardCollection rewardCollection;
        
        public static GameManager Instance { get; private set; }
        
        public PlayerData Data { get; private set; }

        public ZoneDataSO ZoneData => zoneData;
        public RewardCollection RewardCollection => rewardCollection;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            // Initial data load from disk.
            Data = PlayerData.Load();
        }

        private void OnEnable()
        {
            GameEvents.OnWheelSpinStarted += HandleRewardEarned;
            GameEvents.OnWheelSpinEnded += HandleGameOver;
        }

        private void OnDisable()
        {
            GameEvents.OnWheelSpinStarted -= HandleRewardEarned;
            GameEvents.OnWheelSpinEnded -= HandleGameOver;
        }
        
        private void HandleRewardEarned(RewardData reward)
        {
            // If it's a bomb;
            if (reward.RewardType == RewardType.Death) 
            {
                Debug.Log("[GameManager] Hit a bomb! Waiting for player decision.");
                return;
            }

            // Normal Reward Flow:
            // 1. Add the reward to data
            Data.AddReward(reward);
            
            // 2. Advance the zone index
            Data.SetZoneIndex(Data.CurrentZoneIndex + 1);
            
            // 3. // Trigger reward given event.
            GameEvents.OnRewardGiven?.Invoke();
            
            Debug.Log($"[GameManager] Reward: {reward.RewardType} | New Zone: {Data.CurrentZoneIndex}");
        }
        
        private void HandleGameOver(RewardData reward)
        {
            if (reward.RewardType == RewardType.Death)
            {
                // Trigger the game lose event.
                GameEvents.OnGameLose?.Invoke();
            }
        }
        
        /// <summary>
        /// Logic for when the player pays to continue after hitting a bomb.
        /// Call this from your Revive Button.
        /// </summary>
        public bool TryReviveWithCash(int cost)
        {
            var success = Data.TrySpendReward(RewardType.Cash, cost);
    
            if (success)
            {
                Debug.Log($"[GameManager] Revive successful. Spent {cost} Cash. Player stays at zone: {Data.CurrentZoneIndex}");
                
                Data.SetZoneIndex(Data.CurrentZoneIndex + 1);
                GameEvents.OnReviveSuccess?.Invoke();
            }
            else
            {
                var currentCash = Data.GetRewardAmount(RewardType.Cash);
                Debug.Log($"[GameManager] Revive failed! Not enough Cash. Required: {cost}, Available: {currentCash}");
            }
    
            return success;
        }
        
    }
}