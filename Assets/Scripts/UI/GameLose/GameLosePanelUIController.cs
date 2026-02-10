using System;
using HUD;
using Player;
using Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.GameLose
{
    public class GameLosePanelUIController : MonoBehaviour
    {
        [SerializeField] private Button returnMainMenuButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private TMP_Text cashValueText;
        [SerializeField] private TMP_Text reviveCostText;
        
        private void OnValidate()
        {
            if (!returnMainMenuButton && !continueButton)
            {
                returnMainMenuButton = GetComponentInChildren<Button>(true);
                continueButton = GetComponentInChildren<Button>(true);
            }
        }

        private void Start()
        {
            GameManager.Instance.Data.OnRewardChanged += UpdateCashValue;
            
            returnMainMenuButton.onClick.AddListener(ReturnMainMenu);
            continueButton.onClick.AddListener(ContinueGame);
            
            InitializeCashValue();
            
            reviveCostText.text = GameManager.Instance.ZoneData.ReviveCost.ToString();

            if (GameManager.Instance.Data.GetRewardAmount(RewardType.Cash) <
                GameManager.Instance.ZoneData.ReviveCost)
            {
                continueButton.interactable = false;
            }
        }

        private void OnDestroy()
        {
            GameManager.Instance.Data.OnRewardChanged -= UpdateCashValue;
            
            returnMainMenuButton.onClick.RemoveAllListeners();
            continueButton.onClick.RemoveAllListeners();
        }

        private void ReturnMainMenu()
        {
            GameManager.Instance.Data.SetZoneIndex(0);
            HUDManager.Instance.ShowMainMenu();
            HUDManager.Instance.HideLoseGame();
        }

        private void ContinueGame()
        {
            try
            {
                var isReviveSuccessful = GameManager.Instance.TryReviveWithCash(GameManager.Instance.ZoneData.ReviveCost);

                if (isReviveSuccessful)
                {
                    HUDManager.Instance.ShowWheelGame();
                    HUDManager.Instance.HideLoseGame();
                }
            }
            catch (Exception e)
            {
                Logger.Error("GameLosePanel", e.Message);
            }
        }
        
        private void InitializeCashValue()
        {
            cashValueText.text = GameManager.Instance.Data.GetRewardAmount(RewardType.Cash).ToString();
        }

        private void UpdateCashValue(RewardType type, int newAmount)
        {
            if (type != RewardType.Cash) return;
            
            cashValueText.text = newAmount.ToString();
        }
    }
}