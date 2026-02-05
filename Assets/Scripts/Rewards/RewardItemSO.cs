using UnityEngine;

namespace Rewards
{
    [CreateAssetMenu(fileName = "New Reward", menuName = "Rewards/Reward Item")]
    public class RewardItemSO : ScriptableObject
    {
        public RewardType type;
        public string rewardName;
        public Sprite icon;
        public GameObject inOutPrefab;
    }
}