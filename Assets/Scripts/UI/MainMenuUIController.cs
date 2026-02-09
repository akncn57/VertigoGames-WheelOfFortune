using HUD;
using Player;
using Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text cashValueText;
        [SerializeField] private Button playGameButton;
        [SerializeField] private Button exitGameButton;
        [Header("Test")]
        [SerializeField] private Button testButton;
        [SerializeField] private int requiredClicks = 3;
        [SerializeField] private float resetTime = 1f;
        
        private int _clickCount;
        private float _lastClickTime;
        
        private void OnValidate()
        {
            if (!playGameButton && !exitGameButton)
            {
                playGameButton = GetComponentInChildren<Button>(true);
                exitGameButton = GetComponentInChildren<Button>(true);
                testButton = GetComponentInChildren<Button>(true);
            }
        }

        private void Start()
        {
            GameManager.Instance.Data.OnRewardChanged += UpdateCashValue;
            
            playGameButton.onClick.AddListener(PlayGame);
            exitGameButton.onClick.AddListener(ExitGame);
            testButton.onClick.AddListener(OnSecretTriggerClicked);
            InitializeCashValue();
        }
        
        private void OnDestroy()
        {
            GameManager.Instance.Data.OnRewardChanged -= UpdateCashValue;
            
            playGameButton.onClick.RemoveListener(PlayGame);
            exitGameButton.onClick.RemoveListener(ExitGame);
            testButton.onClick.RemoveListener(OnSecretTriggerClicked);
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
        
        private void OnSecretTriggerClicked()
        {
            var currentTime = Time.time;

            if (currentTime - _lastClickTime > resetTime)
            {
                _clickCount = 0;
            }

            _clickCount++;
            _lastClickTime = currentTime;

            if (_clickCount >= requiredClicks)
            {
                _clickCount = 0;
                HUDManager.Instance.ShowTestCanvas();
                HUDManager.Instance.HideMainMenu();
            }
        }
    }
}