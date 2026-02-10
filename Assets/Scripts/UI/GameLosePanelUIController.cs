using System;
using HUD;
using Player;
using Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wheel;

namespace UI
{
    public class GameLosePanelUIController : MonoBehaviour
    {
        [SerializeField] private Button returnMainMenuButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private TMP_Text cashValueText;
        
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
        }

        private void OnDestroy()
        {
            GameManager.Instance.Data.OnRewardChanged -= UpdateCashValue;
            
            returnMainMenuButton.onClick.RemoveAllListeners();
            continueButton.onClick.RemoveAllListeners();
        }

        private void ReturnMainMenu()
        {
            HUDManager.Instance.HideWheelGame();
            HUDManager.Instance.ShowMainMenu();
        }

        private void ContinueGame()
        {
            try
            {
                var isReviveSuccessful = GameManager.Instance.TryReviveWithCash(200);

                if (isReviveSuccessful)
                {
                    gameObject.SetActive(false);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
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