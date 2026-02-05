using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Wheel
{
    public class ZoneBarUIController : MonoBehaviour
    {
        [SerializeField] private ZoneDataSO zoneData;
        [SerializeField] private TMP_Text zoneTextPrefab;
        [SerializeField] private RectTransform zoneContent;
        [SerializeField] private Transform zoneNumberInitPoint;
        [SerializeField] private Image zoneNumberBackground;
        [SerializeField] private Sprite zoneNumberNormalBackgroundSprite;
        [SerializeField] private Sprite zoneNumberSafeBackgroundSprite;
        [SerializeField] private Sprite zoneNumberSuperBackgroundSprite;
        [SerializeField] private float spacing = 16f;
        [SerializeField] private float animationDuration = 1f;
        [SerializeField] private Ease animationEase = Ease.OutBack;
        
        private float _itemWidth;
        private float _startOffsetX;

        private void Awake()
        {
            _itemWidth = zoneTextPrefab.rectTransform.rect.width;
            _startOffsetX = zoneContent.anchoredPosition.x;
        }

        private void Start()
        {
            InitialZoneNumbers();

            WheelEvents.OnZoneChanged += MoveToNextZone;
        }

        private void OnDestroy()
        {
            WheelEvents.OnZoneChanged -= MoveToNextZone;
        }

        private void InitialZoneNumbers()
        {
            for (var i = 0; i < zoneData.ZoneItems.Count; i++)
            {
                var text = Instantiate(zoneTextPrefab, zoneNumberInitPoint);
                var newNumber = i + 1;
                text.text = newNumber.ToString();

                text.color = zoneData.ZoneItems[i].ZoneType switch
                {
                    ZoneType.Safe => Color.green,
                    ZoneType.Super => Color.yellow,
                    _ => Color.white
                };
            }
        }

        private void MoveToNextZone(int targetIndex)
        {
            var step = _itemWidth + spacing;
            var targetX = _startOffsetX - (targetIndex * step);

            zoneNumberBackground.sprite = zoneData.ZoneItems[targetIndex].ZoneType switch
            {
                ZoneType.Safe => zoneNumberSafeBackgroundSprite,
                ZoneType.Super => zoneNumberSuperBackgroundSprite,
                _ => zoneNumberNormalBackgroundSprite
            };

            zoneContent.DOAnchorPosX(targetX, animationDuration).SetEase(animationEase);
        }
    }
}
