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
        [SerializeField] private RectTransform ui_rect_wheel_content;
        [SerializeField] private ZoneDataSO zoneData;
        [SerializeField] private Image wheelImage;
        [SerializeField] private Image wheelIndicatorImage;
        [SerializeField] private Sprite normaZoneWheelSprite;
        [SerializeField] private Sprite safeZoneWheelSprite;
        [SerializeField] private Sprite superZoneWheelSprite;
        [SerializeField] private Sprite normaZoneWheelIndicatorSprite;
        [SerializeField] private Sprite safeZoneWheelIndicatorSprite;
        [SerializeField] private Sprite superZoneWheelIndicatorSprite;
        
        [Header("Spin Settings")]
        [SerializeField] private float spinDuration = 3f;
        [SerializeField] private int fullSpins = 5;
        [SerializeField] private Ease spinEase = Ease.OutQuart;

        private bool _isSpinning = false;
        private List<RewardData> _currentZoneRewards = new List<RewardData>();

        private void Awake()
        {
            _currentZoneRewards = zoneData.ZoneItems[GameManager.Instance.Data.CurrentZoneIndex].ZoneRewards;
        }

        private void Start()
        {
            SetWheel();
        }

        public void Spin()
        {
            if (_isSpinning) return;
            
            _isSpinning = true;
            
            var randomIndex = Random.Range(0, _currentZoneRewards.Count);
            var selectedReward = _currentZoneRewards[randomIndex];
            var targetAngle = (fullSpins * 360) + (randomIndex * 45);

            
            ui_rect_wheel_content.localRotation = Quaternion.Euler(0, 0, 0);
            ui_rect_wheel_content.DORotate(new Vector3(0, 0, -targetAngle), spinDuration, RotateMode.FastBeyond360)
                .SetEase(spinEase)
                .OnComplete(() => 
                {
                    _isSpinning = false;
                    WheelEvents.OnSpinEnded?.Invoke(selectedReward);
                    SetWheel();
                });
        }

        private void SetWheel()
        {
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
            }
        }
    }
}