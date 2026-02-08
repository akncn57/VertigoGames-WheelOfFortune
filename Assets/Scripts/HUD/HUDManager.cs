using UnityEngine;

namespace HUD
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuCanvas;
        [SerializeField] private GameObject wheelGameCanvas;
        [SerializeField] private GameObject testCanvas;
        
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
            mainMenuCanvas.SetActive(true);
        }

        public void HideMainMenu()
        {
            mainMenuCanvas.SetActive(false);
        }

        public void ShowWheelGame()
        {
            wheelGameCanvas.SetActive(true);
        }

        public void HideWheelGame()
        {
            wheelGameCanvas.SetActive(false);
        }

        public void ShowTestCanvas()
        {
            testCanvas.SetActive(true);
        }

        public void HideTestCanvas()
        {
            testCanvas.SetActive(false);
        }
    }
}