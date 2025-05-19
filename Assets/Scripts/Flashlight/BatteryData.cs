using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Battery")]
public class BatteryData : ScriptableObject
{
    public string batteryID;
    public string displayName;
    public float restoreAmount = 100f;
    public Sprite icon;
}