using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SessionRewards
{
    public class SessionRewardItemUI : MonoBehaviour
    {
        [SerializeField] private Image rewardIcon;
        [SerializeField] private TextMeshProUGUI amountText;
        
        public void SetupItem(Sprite rewardSprite, int value)
        {
            rewardIcon.sprite = rewardSprite;
            amountText.text = "x" + value;
        }
    }
}