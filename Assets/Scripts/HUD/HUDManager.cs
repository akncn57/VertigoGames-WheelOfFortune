using UnityEngine;

namespace HUD
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private GameObject wheelGamePanel;
        [SerializeField] private GameObject loseGamePanel;
        [SerializeField] private GameObject testPanel;
        
        public static HUDManager Instance {get; private set;}

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void ShowMainMenu()
        {
            mainMenuPanel.SetActive(true);
        }

        public void HideMainMenu()
        {
            mainMenuPanel.SetActive(false);
        }

        public void ShowInventoryPanel()
        {
            inventoryPanel.SetActive(true);
        }

        public void HideInventoryPanel()
        {
            inventoryPanel.SetActive(false);
        }

        public void ShowWheelGame()
        {
            wheelGamePanel.SetActive(true);
        }

        public void HideWheelGame()
        {
            wheelGamePanel.SetActive(false);
        }

        public void ShowLoseGame()
        {
            loseGamePanel.SetActive(true);
        }

        public void HideLoseGame()
        {
            loseGamePanel.SetActive(false);
        }

        public void ShowTestCanvas()
        {
            testPanel.SetActive(true);
        }

        public void HideTestCanvas()
        {
            testPanel.SetActive(false);
        }
    }
}