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
            WheelEvents.OnSpinStarted += HandleRewardEarned;
            WheelEvents.OnSpinEnded += HandleGameOver;
        }

        private void OnDisable()
        {
            WheelEvents.OnSpinStarted -= HandleRewardEarned;
            WheelEvents.OnSpinEnded -= HandleGameOver;
        }

        private void HandleRewardEarned(RewardData reward)
        {
            Data.SetZoneIndex(Data.CurrentZoneIndex + 1);
            
            if (reward.RewardType == RewardType.Death) return;

            Data.AddReward(reward);
            
            WheelEvents.OnRewardGiven?.Invoke();
            
            Debug.Log($"<color=green><b>[REWARD RECEIVED]</b></color> Type: {reward.RewardType}, Amount: {reward.RewardCount} | Current Zone: {Data.CurrentZoneIndex}");
        }

        private void HandleGameOver(RewardData reward)
        {
            if (reward.RewardType != RewardType.Death) return;
            
            Debug.Log("BOOM! Bomb exploded. Game Over.");
            WheelEvents.OnGameLose?.Invoke();
        }
    }
}