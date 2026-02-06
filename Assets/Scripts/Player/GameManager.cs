using UnityEngine;
using Wheel;
using Rewards;

namespace Player
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public PlayerData Data { get; private set; } = new PlayerData();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            WheelEvents.OnSpinEnded += HandleRewardEarned;
        }

        private void OnDisable()
        {
            WheelEvents.OnSpinEnded -= HandleRewardEarned;
        }

        private void HandleRewardEarned(RewardData reward)
        {
            if (reward.RewardType == RewardType.Death)
            {
                HandleGameOver();
                return;
            }

            Data.AddReward(reward);
            Data.CurrentZoneIndex++;
            
            Debug.Log($"<color=green><b>[REWARD RECEIVED]</b></color> Type: {reward.RewardType}, Amount: {reward.RewardCount} | Current Zone: {Data.CurrentZoneIndex}");
            
            WheelEvents.OnZoneChanged?.Invoke(Data.CurrentZoneIndex);
        }

        private void HandleGameOver()
        {
            Debug.Log("BOOM! Bomb exploded. Game Over.");
        }
    }
}