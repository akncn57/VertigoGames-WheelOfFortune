using HUD;
using Player;
using Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Test
{
    public class TestPanelUIController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_InputField inputFieldZoneIndex;
        [SerializeField] private TMP_InputField inputFieldCashValue;
        [SerializeField] private TMP_Text zoneIndexPlaceholderText;
        [SerializeField] private TMP_Text cashValuePlaceholderText;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button returnMenuButton;

        private void Awake()
        {
            saveButton.onClick.AddListener(SetZoneIndex);
            returnMenuButton.onClick.AddListener(() =>
            {
                HUDManager.Instance.ShowMainMenu();
                HUDManager.Instance.HideTestCanvas();
            });
        }

        private void Start()
        {
            GameManager.Instance.Data.OnZoneIndexChanged += OnZoneIndexChanged;
            GameManager.Instance.Data.OnRewardChanged += OnRewardDataChanged;

            zoneIndexPlaceholderText.text = GameManager.Instance.Data.CurrentZoneIndex.ToString();
            cashValuePlaceholderText.text = GameManager.Instance.Data.GetRewardAmount(RewardType.Cash).ToString();
        }

        private void OnDestroy()
        {
            GameManager.Instance.Data.OnZoneIndexChanged -= OnZoneIndexChanged;
            GameManager.Instance.Data.OnRewardChanged -= OnRewardDataChanged;
        }

        private void OnZoneIndexChanged(int newValue)
        {
            zoneIndexPlaceholderText.text = newValue.ToString();
        }

        private void OnRewardDataChanged(RewardType rewardType, int newValue)
        {
            if (rewardType != RewardType.Cash) return;
            
            cashValuePlaceholderText.text = newValue.ToString();
        }

        private void SetZoneIndex()
        {
            if (int.TryParse(inputFieldZoneIndex.text, out var newIndex))
            {
                GameManager.Instance.Data.SetZoneIndex(newIndex - 1);
                GameManager.Instance.Data.Save();
            }
            
            if (int.TryParse(inputFieldCashValue.text, out var newValue))
            {
                GameManager.Instance.Data.AddReward(new RewardData{RewardType = RewardType.Cash, RewardCount = newValue});
            }
        }
    }
}