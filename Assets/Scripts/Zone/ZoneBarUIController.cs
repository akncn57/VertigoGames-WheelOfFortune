using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wheel;

namespace Zone
{
    public class ZoneBarUIController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private ZoneDataSO zoneData;

        [Header("Prefabs")]
        [SerializeField] private TMP_Text uiTextZoneNumberValuePrefab;

        [Header("UI Elements")]
        [SerializeField] private RectTransform uiRectZoneContainer;
        [SerializeField] private Image uiImageZoneBackgroundValue;
        
        [Header("Settings")]
        [SerializeField] private float spacing = 16f;
        [SerializeField] private float animationDuration = 1f;
        [SerializeField] private Ease animationEase = Ease.OutBack;

        [Header("Zone Visuals - Sprites")]
        [SerializeField] private Sprite spriteZoneNormal;
        [SerializeField] private Sprite spriteZoneSafe;
        [SerializeField] private Sprite spriteZoneSuper;

        [Header("Zone Visuals - Colors")]
        [SerializeField] private Color colorZoneNormal = Color.white;
        [SerializeField] private Color colorZoneSafe = Color.green;
        [SerializeField] private Color colorZoneSuper = Color.yellow;

        private float _itemWidth;
        private float _startOffsetX;

        private void Awake()
        {
            // Get text item width and start offset for content movement.
            _itemWidth = uiTextZoneNumberValuePrefab.rectTransform.rect.width;
            _startOffsetX = uiRectZoneContainer.anchoredPosition.x;
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
                // Instantiate zone text object in content.
                var zoneText = Instantiate(uiTextZoneNumberValuePrefab, uiRectZoneContainer);
                
                // Change TMP settings.
                zoneText.name = $"ui_text_zone_{i + 1}_value"; 
                zoneText.text = (i + 1).ToString();
                zoneText.color = GetZoneColor(zoneData.ZoneItems[i].ZoneType);
            }
        }

        private void MoveToNextZone(int targetIndex)
        {
            // Get content move parameters
            var step = _itemWidth + spacing;
            var targetX = _startOffsetX - (targetIndex * step);

            UpdateZoneVisuals(targetIndex);

            // Move content.
            uiRectZoneContainer.DOAnchorPosX(targetX, animationDuration).SetEase(animationEase);
        }

        private void UpdateZoneVisuals(int index)
        {
            var zoneType = zoneData.ZoneItems[index].ZoneType;

            uiImageZoneBackgroundValue.sprite = zoneType switch
            {
                ZoneType.Safe => spriteZoneSafe,
                ZoneType.Super => spriteZoneSuper,
                _ => spriteZoneNormal
            };
        }

        private Color GetZoneColor(ZoneType type) => type switch
        {
            ZoneType.Safe => colorZoneSafe,
            ZoneType.Super => colorZoneSuper,
            _ => colorZoneNormal
        };
    }
}