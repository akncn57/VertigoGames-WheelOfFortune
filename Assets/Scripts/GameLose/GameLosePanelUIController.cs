using HUD;
using UnityEngine;
using UnityEngine.UI;
using Wheel;

namespace GameLose
{
    public class GameLosePanelUIController : MonoBehaviour
    {
        [SerializeField] private Button returnMainMenuButton;
        [SerializeField] private Button continueButton;
        
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
            returnMainMenuButton.onClick.AddListener(ReturnMainMenu);
            continueButton.onClick.AddListener(ContinueGame);
        }

        private void OnDestroy()
        {
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
            WheelEvents.OnContinueGame?.Invoke();
            gameObject.SetActive(false);
        }
    }
}