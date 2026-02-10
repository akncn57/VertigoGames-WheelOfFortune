using System;
using DG.Tweening;
using Player;
using Rewards;
using UI.InOut;
using UnityEngine;
using Wheel;

namespace InOut
{
    public class InOutController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InOutItemUI inOutPrefab;
        [SerializeField] private Transform uiTransformOverlayContainer;

        [Header("Animation Settings")]
        [SerializeField] private float punchDuration = 0.5f;
        [SerializeField] private float moveDuration = 0.8f;
        [SerializeField] private Ease moveEase = Ease.InBack;
        
        public static InOutController Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        
        public void PlayInOut(RewardData rewardData, Vector3 startPos, Vector3 targetPos, Action onComplete)
        {
            GameEvents.OnInOutsStarted?.Invoke();
            
            var inOutObject = Instantiate(inOutPrefab, uiTransformOverlayContainer);
            
            inOutObject.SetupItem(GameManager.Instance.RewardCollection.GetRewardByType(rewardData.RewardType).icon, rewardData.RewardCount);
            inOutObject.transform.position = startPos;
            inOutObject.transform.localScale = Vector3.zero;

            var flowSequence = DOTween.Sequence();

            flowSequence.Append(inOutObject.transform.DOScale(1.2f, punchDuration).SetEase(Ease.OutBack));
            flowSequence.AppendInterval(0.3f);
            flowSequence.Append(inOutObject.transform.DOMove(targetPos, moveDuration).SetEase(moveEase));
            flowSequence.Join(inOutObject.transform.DOScale(0.4f, moveDuration));

            flowSequence.OnComplete(() =>
            {
                onComplete?.Invoke();
                GameEvents.OnInOutsEnded?.Invoke();
                Destroy(inOutObject.gameObject);
            });
        }
    }
}