using System.Collections.Generic;
using HUD;
using Player;
using Rewards;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryPanelUIController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InventoryItemUI itemPrefab;
        [SerializeField] private Transform container;
        [SerializeField] private Button returnMenuButton;

        private readonly List<InventoryItemUI> _spawnedItems = new List<InventoryItemUI>();

        private void OnEnable()
        {
            RefreshInventory();
            returnMenuButton.onClick.AddListener(ReturnMenu);

            if (GameManager.Instance != null && GameManager.Instance.Data != null)
            {
                GameManager.Instance.Data.OnRewardChanged += HandleRewardChanged;
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null && GameManager.Instance.Data != null)
            {
                GameManager.Instance.Data.OnRewardChanged -= HandleRewardChanged;
            }
        }
        
        private void RefreshInventory()
        {
            ClearContainer();

            var rewards = GameManager.Instance.Data.CollectedRewards;

            foreach (var reward in rewards)
            {
                CreateItemUI(reward);
            }
        }

        private void CreateItemUI(RewardData data)
        {
            var newItem = Instantiate(itemPrefab, container);
            var icon = GameManager.Instance.RewardCollection.GetRewardByType(data.RewardType).icon;
            
            newItem.SetItem(icon, data.RewardCount);
            _spawnedItems.Add(newItem);
        }

        private void HandleRewardChanged(RewardType type, int newAmount)
        {
            RefreshInventory();
        }

        private void ClearContainer()
        {
            foreach (var item in _spawnedItems)
            {
                if (item != null) Destroy(item.gameObject);
            }
            
            _spawnedItems.Clear();
        }

        private void ReturnMenu()
        {
            HUDManager.Instance.ShowMainMenu();
            HUDManager.Instance.HideInventoryPanel();
        }
    }
}