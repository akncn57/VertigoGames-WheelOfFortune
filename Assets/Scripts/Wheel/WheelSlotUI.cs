using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Wheel
{
    public class WheelSlotUI : MonoBehaviour
    {
        [SerializeField] private Image rewardIcon;
        [SerializeField] private TextMeshProUGUI amountText;

        public void SetupSlot(Sprite rewardSprite, int value)
        {
            rewardIcon.sprite = rewardSprite;

            if (value <= 1)
                amountText.text = "";
            else
                amountText.text = "x" + value;
        }
    }
}