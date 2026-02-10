using System;
using System.Collections;
using System.Collections.Generic;
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
        
        private Sequence _currentSequence;
        
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
        
        public IEnumerator PlayInOutCor(RewardData rewardData, Vector3 startPos, Vector3 targetPos, Action onComplete)
        {
            _currentSequence?.Kill();
            _currentSequence = DOTween.Sequence();
            
            GameEvents.OnInOutsStarted?.Invoke();
            
            var inOutObject = Instantiate(inOutPrefab, uiTransformOverlayContainer);
            
            inOutObject.SetupItem(GameManager.Instance.RewardCollection.GetRewardByType(rewardData.RewardType).icon, rewardData.RewardCount);
            inOutObject.transform.position = startPos;
            inOutObject.transform.localScale = Vector3.zero;

            _currentSequence.Append(inOutObject.transform.DOScale(1.2f, punchDuration).SetEase(Ease.OutBack));
            _currentSequence.AppendInterval(0.3f);
            _currentSequence.Append(inOutObject.transform.DOMove(targetPos, moveDuration).SetEase(moveEase));
            _currentSequence.Join(inOutObject.transform.DOScale(0.4f, moveDuration));

            yield return _currentSequence.WaitForCompletion();
            
            onComplete?.Invoke();
            GameEvents.OnInOutsEnded?.Invoke();
            Destroy(inOutObject.gameObject);
        }
        
        public void PlayInOut(RewardData rewardData, Vector3 startPos, Vector3 targetPos, Action onComplete)
        {
            _currentSequence?.Kill();
            _currentSequence = DOTween.Sequence();
            
            GameEvents.OnInOutsStarted?.Invoke();
            
            var inOutObject = Instantiate(inOutPrefab, uiTransformOverlayContainer);
            
            inOutObject.SetupItem(GameManager.Instance.RewardCollection.GetRewardByType(rewardData.RewardType).icon, rewardData.RewardCount);
            inOutObject.transform.position = startPos;
            inOutObject.transform.localScale = Vector3.zero;

            _currentSequence.Append(inOutObject.transform.DOScale(1.2f, punchDuration).SetEase(Ease.OutBack));
            _currentSequence.AppendInterval(0.3f);
            _currentSequence.Append(inOutObject.transform.DOMove(targetPos, moveDuration).SetEase(moveEase));
            _currentSequence.Join(inOutObject.transform.DOScale(0.4f, moveDuration));

            _currentSequence.OnComplete(() =>
            {
                onComplete?.Invoke();
                GameEvents.OnInOutsEnded?.Invoke();
                Destroy(inOutObject.gameObject);
            });
        }
    }
}