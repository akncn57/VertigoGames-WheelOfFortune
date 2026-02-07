using System;
using System.Collections.Generic;
using DG.Tweening;
using Player;
using Rewards;
using UnityEngine;
using UnityEngine.UI;
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
            WheelEvents.OnSpinEnded += EnableSpinButton;
            spinButton.onClick.AddListener(Spin);
        }

        private void OnDisable()
        {
            WheelEvents.OnSpinEnded -= EnableSpinButton;
            spinButton.onClick.RemoveAllListeners();
        }

        private void Start()
        {
            PrepareWheel();
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
        }

        private void EnableSpinButton(RewardData selectedReward)
        {
            spinButton.interactable = true;
        }
    }
}