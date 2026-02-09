using UnityEngine;
using Wheel;
using Rewards;

namespace Player
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ZoneDataSO zoneData;
        [SerializeField] private RewardCollection rewardCollection;
        
        public static GameManager Instance { get; private set; }
        public PlayerData Data { get; private set; } = new();
        public ZoneDataSO ZoneData => zoneData;
        public RewardCollection RewardCollection => rewardCollection;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
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
            Data.SetZoneIndex(Data.CurrentZoneIndex + 1);
            
            if (reward.RewardType == RewardType.Death) return;

            Data.AddReward(reward);
            
            GameEvents.OnRewardGiven?.Invoke();
            
            Debug.Log($"<color=green><b>[REWARD RECEIVED]</b></color> Type: {reward.RewardType}, Amount: {reward.RewardCount} | Current Zone: {Data.CurrentZoneIndex}");
        }

        private void HandleGameOver(RewardData reward)
        {
            if (reward.RewardType != RewardType.Death) return;
            
            Debug.Log("BOOM! Bomb exploded. Game Over.");
            GameEvents.OnGameLose?.Invoke();
        }
    }
}