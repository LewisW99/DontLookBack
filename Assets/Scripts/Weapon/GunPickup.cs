using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public WeaponData weaponToGive;

    private void OnTriggerEnter(Collider other)
    {
        var weaponHandler = other.GetComponentInChildren<PlayerWeaponHandler>();
        if (weaponHandler != null)
        {
            weaponHandler.canUseGun = true;
            weaponHandler.EquipWeapon(weaponToGive);
            Destroy(gameObject);
        }
    }
}
