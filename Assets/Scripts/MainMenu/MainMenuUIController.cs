using HUD;
using Player;
using Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text cashValueText;
        [SerializeField] private Button playGameButton;
        [SerializeField] private Button exitGameButton;
        
        private void OnValidate()
        {
            if (!playGameButton && !exitGameButton)
            {
                playGameButton = GetComponentInChildren<Button>(true);
                exitGameButton = GetComponentInChildren<Button>(true);
            }
        }

        private void Start()
        {
            GameManager.Instance.Data.OnRewardChanged += UpdateCashValue;
            
            playGameButton.onClick.AddListener(PlayGame);
            exitGameButton.onClick.AddListener(ExitGame);
            InitializeCashValue();
        }
        
        private void OnDestroy()
        {
            GameManager.Instance.Data.OnRewardChanged -= UpdateCashValue;
            
            playGameButton.onClick.RemoveAllListeners();
            exitGameButton.onClick.RemoveAllListeners();
        }

        private void PlayGame()
        {
            HUDManager.Instance.HideMainMenu();
            HUDManager.Instance.ShowWheelGame();
        }

        private void ExitGame()
        {
            Application.Quit();
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