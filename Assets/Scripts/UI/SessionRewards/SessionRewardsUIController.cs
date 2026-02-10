using System.Collections.Generic;
using DG.Tweening;
using HUD;
using InOut;
using Player;
using Rewards;
using UnityEngine;
using UnityEngine.UI;
using Wheel;

namespace UI.SessionRewards
{
    public class SessionRewardsUIController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SessionRewardItemUI uiPrefabRewardItem;
        [SerializeField] private Transform uiTransformItemContainer;
        [SerializeField] private Transform inOutStartPoint;
        [SerializeField] private Button exitButton;

        private readonly Dictionary<RewardType, SessionRewardItemUI> _collectedItems = new();
        private readonly Dictionary<RewardType, int> _sessionRewards = new();
        
        private void OnEnable()
        {
            GameEvents.OnWheelSpinEnded += HandleRewardCollected;
            GameEvents.OnWheelSpinStarted += DisableExitButton;
            ResetSessionUI();
            exitButton.onClick.AddListener(ExitGame);
        }

        private void OnDisable()
        {
            GameEvents.OnWheelSpinEnded -= HandleRewardCollected;
            GameEvents.OnWheelSpinStarted -= DisableExitButton;
        }

        private void HandleRewardCollected(RewardData rewardData)
        {
            if (rewardData.RewardType == RewardType.Death) return;
            
            SessionRewardItemUI targetUI;
            var isFirstTime = !_sessionRewards.ContainsKey(rewardData.RewardType);

            if (isFirstTime)
            {
                // Initialize data and reserve UI slot
                _sessionRewards.Add(rewardData.RewardType, 0);
        
                targetUI = Instantiate(uiPrefabRewardItem, uiTransformItemContainer);
                targetUI.name = $"ui_item_{rewardData.RewardType}_value";
        
                // Setup initial empty state
                targetUI.SetupItem(GameManager.Instance.RewardCollection.GetRewardByType(rewardData.RewardType).icon, 0);
                _collectedItems.Add(rewardData.RewardType, targetUI);
                
                // Ensure UI Layout updates immediately to get the correct world position for animation.
                Canvas.ForceUpdateCanvases();
                LayoutRebuilder.ForceRebuildLayoutImmediate(uiTransformItemContainer.GetComponent<RectTransform>());
            }
            else
            {
                targetUI = _collectedItems[rewardData.RewardType];
            }

            // Start transfer animation
            var startPos = inOutStartPoint; 
            
            // BUG: If target pos out of the rewards content, inout object flying to void.
            var targetPos = targetUI.transform.position;

            InOutController.Instance.PlayInOut(
                rewardData,
                startPos.position,
                targetPos,
                () => 
                {
                    // Update data on arrival
                    _sessionRewards[rewardData.RewardType] += rewardData.RewardCount;
            
                    // Refresh UI with new amount
                    targetUI.SetupItem(
                        GameManager.Instance.RewardCollection.GetRewardByType(rewardData.RewardType).icon, 
                        _sessionRewards[rewardData.RewardType]
                    );

                    // Play feedback punch effect
                    targetUI.transform.DOPunchScale(Vector3.one * 0.15f, 0.3f, 5, 1f);

                    EnableExitButton();
                }
            );
        }
        
        private void ResetSessionUI()
        {
            _sessionRewards.Clear();
            _collectedItems.Clear();

            foreach (Transform child in uiTransformItemContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void EnableExitButton()
        {
            exitButton.interactable = true;
        }
        
        private void DisableExitButton(RewardData rewardData)
        {
            exitButton.interactable = false;
        }

        private void ExitGame()
        {
            GameManager.Instance.DepositRewards();
            HUDManager.Instance.ShowMainMenu();
            HUDManager.Instance.HideWheelGame();
        }
    }
}