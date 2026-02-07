using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Wheel
{
    public class WheelSlotUI : MonoBehaviour
    {
        [SerializeField] private Image rewardIcon;
        [SerializeField] private TextMeshProUGUI amountText;

        /// <summary>
        /// Updates the slot's visual elements with the provided reward sprite and amount.
        /// </summary>
        /// <param name="rewardSprite">The sprite to be displayed as the reward icon.</param>
        /// <param name="value">The quantity of the reward.</param>
        public void SetupSlot(Sprite rewardSprite, int value)
        {
            rewardIcon.sprite = rewardSprite;

            if (value <= 1)
            {
                amountText.text = "";
            }
            else
            {
                amountText.text = "x" + value;
            }
        }
    }
}