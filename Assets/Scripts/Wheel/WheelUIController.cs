using System;
using System.Collections.Generic;
using DG.Tweening;
using Player;
using Rewards;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Wheel
{
    public class WheelUIController: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform ui_rect_wheel_content;
        [SerializeField] private ZoneDataSO zoneData;
        
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
                });
        }
    }
}