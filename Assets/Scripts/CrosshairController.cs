using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Image crosshairImage; // Assign in Inspector
    public PlayerWeaponHandler weaponHandler; // Assign in Inspector

    
    void Start()
    {
        if (weaponHandler == null)
            weaponHandler = FindObjectOfType<PlayerWeaponHandler>();
    }
    void Update()
    {
        if (weaponHandler != null && crosshairImage != null)
        {
            crosshairImage.enabled = weaponHandler.canUseGun;
        }
    }
}