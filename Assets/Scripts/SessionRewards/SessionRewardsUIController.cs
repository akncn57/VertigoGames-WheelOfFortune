using System.Collections.Generic;
using Rewards;
using UnityEngine;
using Wheel;

namespace SessionRewards
{
    public class SessionRewardsUIController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RewardCollection rewardCollection;
        [SerializeField] private SessionRewardItemUI uiPrefabRewardItem;
        [SerializeField] private Transform uiTransformItemContainer;

        private readonly Dictionary<RewardType, SessionRewardItemUI> _collectedItems = new();
        private readonly Dictionary<RewardType, int> _sessionRewards = new();
        
        private void OnEnable()
        {
            WheelEvents.OnSpinEnded += HandleRewardCollected;
        }

        private void OnDisable()
        {
            WheelEvents.OnSpinEnded -= HandleRewardCollected;
        }

        private void HandleRewardCollected(RewardData rewardData)
        {
            // If session rewards contains collected reward;
            if (_sessionRewards.ContainsKey(rewardData.RewardType))
            {
                // Change session reward value.
                _sessionRewards[rewardData.RewardType] += rewardData.RewardCount;
                
                var uiItem = _collectedItems[rewardData.RewardType];
                
                uiItem.SetupItem(rewardCollection.GetRewardByType(rewardData.RewardType).icon, _sessionRewards[rewardData.RewardType]);
            }
            else
            {
                // Add collected reward in session rewards.
                _sessionRewards.Add(rewardData.RewardType, rewardData.RewardCount);
                
                // Create item prefab in container.
                var newItem = Instantiate(uiPrefabRewardItem, uiTransformItemContainer);
                
                // Setup item UI.
                newItem.SetupItem(rewardCollection.GetRewardByType(rewardData.RewardType).icon, rewardData.RewardCount);
                
                // Add collected reward in collected rewards.
                _collectedItems.Add(rewardData.RewardType, newItem);
            }
        }
    }
}