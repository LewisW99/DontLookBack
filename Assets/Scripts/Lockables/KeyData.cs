using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Key")]
public class KeyData : ScriptableObject
{
    public string keyID;
    public string displayName;
    public Sprite icon;
}
