using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform keyListParent;
    [SerializeField] private Transform batteryListParent;
    [SerializeField] private GameObject itemDisplayPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        bool isOpen = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isOpen);

        if (!isOpen)
        {
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        ClearChildren(keyListParent);
        ClearChildren(batteryListParent);

        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();

        foreach (var key in inventory.collectedKeys)
        {
            CreateItemUI(key.displayName, key.icon, keyListParent);
        }

        foreach (var battery in inventory.collectedBatteries)
        {
            CreateItemUI(battery.displayName, battery.icon, batteryListParent);
        }
        
        
    }

    private void CreateItemUI(string name, Sprite icon, Transform parent)
    {
        GameObject itemUI = Instantiate(itemDisplayPrefab, parent);
        itemUI.GetComponentInChildren<TextMeshProUGUI>().text = name;
        itemUI.GetComponentInChildren<Image>().sprite = icon;
    }

    private void ClearChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}