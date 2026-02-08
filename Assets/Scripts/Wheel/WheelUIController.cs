using System;
using System.Collections.Generic;
using DG.Tweening;
using Player;
using Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zone;
using Random = UnityEngine.Random;

namespace Wheel
{
    public class WheelUIController: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform wheelContent;
        [SerializeField] private ZoneDataSO zoneData;
        [SerializeField] private RewardCollection rewardCollection;
        [SerializeField] private Image wheelImage;
        [SerializeField] private Image wheelIndicatorImage;
        [SerializeField] private Button spinButton;
        [SerializeField] private TMP_Text safeZoneDescriptionText;
        [SerializeField] private TMP_Text superZoneDescriptionText;
        [SerializeField] private GameObject gameLosePanel;
        
        [Header("Zone Sprites")]
        [SerializeField] private Sprite normaZoneWheelSprite;
        [SerializeField] private Sprite safeZoneWheelSprite;
        [SerializeField] private Sprite superZoneWheelSprite;
        [SerializeField] private Sprite normaZoneWheelIndicatorSprite;
        [SerializeField] private Sprite safeZoneWheelIndicatorSprite;
        [SerializeField] private Sprite superZoneWheelIndicatorSprite;
        
        [Header("Zone Slots")]
        [SerializeField] private List<WheelSlotUI> wheelSlots = new List<WheelSlotUI>();
        
        [Header("Spin Settings")]
        [SerializeField] private float spinDuration = 3f;
        [SerializeField] private int fullSpins = 5;
        [SerializeField] private Ease spinEase = Ease.OutQuart;

        private List<RewardData> _currentZoneRewards = new List<RewardData>();

        private void OnValidate()
        {
            if (!spinButton)
            {
                spinButton = GetComponentInChildren<Button>(true);
            }
        }

        private void OnEnable()
        {
            WheelEvents.OnInOutEnded += EnableSpinButton;
            WheelEvents.OnGameLose += EnableGameLosePanel;
            WheelEvents.OnContinueGame += PrepareWheel;
            spinButton.onClick.AddListener(Spin);
        }

        private void OnDisable()
        {
            WheelEvents.OnInOutEnded -= EnableSpinButton;
            WheelEvents.OnGameLose -= EnableGameLosePanel;
            WheelEvents.OnContinueGame -= PrepareWheel;
            spinButton.onClick.RemoveAllListeners();
        }

        private void Start()
        {
            PrepareWheel();
            UpdateZoneDescriptions();
        }

        private void PrepareWheel()
        {
            UpdateZoneData();
            RefreshWheelVisuals();
        }

        private void Spin()
        {
            // First, close spin button interactable for anti-spam.
            spinButton.interactable = false;
            
            // Get random reward.
            var randomIndex = Random.Range(0, _currentZoneRewards.Count);
            var selectedReward = _currentZoneRewards[randomIndex];
            var targetAngle = (fullSpins * 360) + (randomIndex * 360 / wheelSlots.Count);
            
            WheelEvents.OnSpinStarted(selectedReward);
            
            // Reset wheel rotation.
            wheelContent.localRotation = Quaternion.Euler(0, 0, 0);
            
            // Spin wheel.
            wheelContent.DORotate(new Vector3(0, 0, targetAngle), spinDuration, RotateMode.FastBeyond360)
                .SetEase(spinEase)
                .OnComplete(() => OnSpinCompleted(selectedReward));
        }

        private void UpdateZoneData()
        {
            _currentZoneRewards = zoneData.ZoneItems[GameManager.Instance.Data.CurrentZoneIndex].ZoneRewards;
            WheelEvents.OnZoneChanged?.Invoke(GameManager.Instance.Data.CurrentZoneIndex);
        }

        private void RefreshWheelVisuals()
        {
            // Set wheel UI.
            switch (zoneData.ZoneItems[GameManager.Instance.Data.CurrentZoneIndex].ZoneType)
            {
                case ZoneType.Normal:
                    wheelImage.sprite = normaZoneWheelSprite;
                    wheelIndicatorImage.sprite = normaZoneWheelIndicatorSprite;
                    break;
                case ZoneType.Safe:
                    wheelImage.sprite = safeZoneWheelSprite;
                    wheelIndicatorImage.sprite = safeZoneWheelIndicatorSprite;
                    break;
                case ZoneType.Super:
                    wheelImage.sprite = superZoneWheelSprite;
                    wheelIndicatorImage.sprite = superZoneWheelIndicatorSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            // Set wheel reward slots UI.
            for (var i = 0; i < wheelSlots.Count; i++)
            {
                wheelSlots[i].SetupSlot(
                    rewardCollection.GetRewardByType(zoneData.ZoneItems[GameManager.Instance.Data.CurrentZoneIndex].ZoneRewards[i].RewardType).icon,
                    zoneData.ZoneItems[GameManager.Instance.Data.CurrentZoneIndex].ZoneRewards[i].RewardCount
                );
            }
        }
        
        private void OnSpinCompleted(RewardData selectedReward)
        {
            WheelEvents.OnSpinEnded?.Invoke(selectedReward);
            
            // Prepare wheel UI after spin.
            UpdateZoneData();
            PrepareWheel();

            // Update description texts.
            UpdateZoneDescriptions();
        }

        private void EnableSpinButton()
        {
            spinButton.interactable = true;
        }
        
        private void UpdateZoneDescriptions()
        {
            var currentIndex = GameManager.Instance.Data.CurrentZoneIndex;
            var nextSafe = -1;
            var nextSuper = -1;

            // Search for the upcoming special zones starting from the next index
            for (var i = currentIndex + 1; i < zoneData.ZoneItems.Count; i++)
            {
                // Store user-friendly zone number (index + 1)
                if (nextSafe == -1 && zoneData.ZoneItems[i].ZoneType == ZoneType.Safe)
                    nextSafe = i + 1;
    
                if (nextSuper == -1 && zoneData.ZoneItems[i].ZoneType == ZoneType.Super)
                    nextSuper = i + 1;

                // Stop searching once both types are found for optimization
                if (nextSafe != -1 && nextSuper != -1) break;
            }

            // Update UI labels or show fallback text if no more special zones exist
            safeZoneDescriptionText.text = nextSafe != -1 
                ? $"Next Safe Zone: {nextSafe}" 
                : "No more Safe Zones";

            superZoneDescriptionText.text = nextSuper != -1 
                ? $"Next Super Zone: {nextSuper}" 
                : "No more Super Zones";
        }

        private void EnableGameLosePanel()
        {
            gameLosePanel.SetActive(true);
        }
    }
}