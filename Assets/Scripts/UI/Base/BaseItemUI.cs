using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Base
{
    public abstract class BaseItemUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text valueText;

        public void SetupItem(Sprite icon, int value)
        {
            iconImage.sprite = icon;
            valueText.text = value.ToString();
        }
    }
}