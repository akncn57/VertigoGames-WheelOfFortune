using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryItemUI : MonoBehaviour
    {
        public Image ItemImage;
        public TMP_Text ItemValueText;

        public void SetItem(Sprite sprite, int value)
        {
            ItemImage.sprite = sprite;
            ItemValueText.text = value.ToString();
        }
    }
}