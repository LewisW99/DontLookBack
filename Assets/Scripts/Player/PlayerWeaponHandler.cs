using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    public bool canUseGun = false;
    public WeaponData startingWeaponData;

    private WeaponBase currentWeapon;

    public Transform gunTransform;
    void Start()
    {
        if (canUseGun && startingWeaponData != null)
        {
            EquipWeapon(startingWeaponData);
        }
    }

    public void EquipWeapon(WeaponData data)
    {
        GameObject weaponInstance = Instantiate(data.weaponPrefab, gunTransform);
        currentWeapon = weaponInstance.GetComponent<WeaponBase>();
        currentWeapon.data = data;
    }

    public void EnableGunUsage()
    {
        canUseGun = true;
        EquipWeapon(startingWeaponData); // or whatever weapon
    }
}
