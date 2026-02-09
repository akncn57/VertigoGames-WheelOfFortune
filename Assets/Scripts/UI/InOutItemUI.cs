using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InOutItemUI : MonoBehaviour
    {
        [SerializeField] private Image rewardIcon;
        [SerializeField] private TMP_Text amountText;
        
        public void SetupInOutItem(Sprite rewardSprite, int value)
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