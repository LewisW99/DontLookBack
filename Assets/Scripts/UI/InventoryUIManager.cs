using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform itemGridParent;

    [SerializeField] private GameObject itemDisplayPrefab;

    private PauseMenu pauseMenu;

    private void Start()
    {
        if(pauseMenu == null)
            pauseMenu = FindFirstObjectByType<PauseMenu>();

    }
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
        if(isOpen)
        {
            
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.isPaused = false;
        }

        if (!isOpen)
        {
            RefreshUI();
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            pauseMenu.isPaused = true;

        }
    }

    public void RefreshUI(System.Type filterType = null)
    {
        ClearChildren(itemGridParent);

        var inventory = FindFirstObjectByType<PlayerInventory>();
        if (inventory == null) return;

        foreach (var item in inventory.GetAllItems())
        {
            if (filterType != null && item.GetType() != filterType) continue;

            var slotGO = Instantiate(itemDisplayPrefab, itemGridParent);
            var slotUI = slotGO.GetComponent<ItemSlotUI>();
            slotUI.Setup(item);
        }
    }

    public void ShowAll() => RefreshUI();
    public void ShowKeys() => RefreshUI(typeof(KeyData));
    public void ShowBatteries() => RefreshUI(typeof(BatteryData));
    public void ShowPhotos() => RefreshUI(typeof(PhotoData));

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