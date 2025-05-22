using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;

    public void Setup(InventoryItemData item)
    {
        nameText.text = item.itemName;
        iconImage.sprite = item.icon;
    }
}
