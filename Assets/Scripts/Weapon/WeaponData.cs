using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;
    public int magazineSize;
    public float fireRate;
    public float damage;
}
