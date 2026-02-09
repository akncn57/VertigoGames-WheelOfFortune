using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wheel;
using Zone;

namespace UI
{
    public class ZoneBarUIController : MonoBehaviour
    {
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
            GameEvents.OnZoneChanged += HandleZoneChanged;
            MoveToNextZone(GameManager.Instance.Data.CurrentZoneIndex, false);
        }

        private void OnDestroy()
        {
            GameEvents.OnZoneChanged -= HandleZoneChanged;
        }

        private void InitialZoneNumbers()
        {
            for (var i = 0; i < GameManager.Instance.ZoneData.ZoneItems.Count; i++)
            {
                // Instantiate zone text object in content.
                var zoneText = Instantiate(uiTextZoneNumberValuePrefab, uiRectZoneContainer);
                
                // Change TMP settings.
                zoneText.name = $"ui_text_zone_{i + 1}_value"; 
                zoneText.text = (i + 1).ToString();
                zoneText.color = GetZoneColor(GameManager.Instance.ZoneData.ZoneItems[i].ZoneType);
            }
        }

        private void HandleZoneChanged(int targetIndex)
        {
            MoveToNextZone(targetIndex, true);
        }

        private void MoveToNextZone(int targetIndex, bool animate)
        {
            var step = _itemWidth + spacing;
            var targetX = _startOffsetX - (targetIndex * step);

            UpdateZoneVisuals(targetIndex);

            if (animate)
            {
                // Smooth move during gameplay
                uiRectZoneContainer.DOAnchorPosX(targetX, animationDuration).SetEase(animationEase);
            }
            else
            {
                // Instant jump for initial setup or menu transitions
                uiRectZoneContainer.anchoredPosition = new Vector2(targetX, uiRectZoneContainer.anchoredPosition.y);
            }
        }

        private void UpdateZoneVisuals(int index)
        {
            var zoneType = GameManager.Instance.ZoneData.ZoneItems[index].ZoneType;

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