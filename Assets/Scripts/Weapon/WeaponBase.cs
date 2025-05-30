using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public WeaponData data;
    [SerializeField] ParticleSystem muzzleFlash;
    private int currentAmmo;
    private float lastShotTime;

    void Start()
    {
        currentAmmo = data.magazineSize;
        if(muzzleFlash == null)
            muzzleFlash = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Fire1 pressed");
            Debug.Log($"Time: {Time.time}, LastShot: {lastShotTime}, Cooldown: {data.fireRate}");

            if (Time.time > lastShotTime + data.fireRate)
            {
                Fire();
            }
            else
            {
                Debug.Log("Fire rate cooldown not finished.");
            }
        }
    }

    void Fire()
    {
        Debug.Log("FIRE FUNCTION CALLED!");

        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.forward;

        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.Log("Hit: " + hit.collider.name);

            // Add a red debug line to visualize the hit
            Debug.DrawLine(origin, hit.point, Color.red, 1f);
        }
        else
        {
            // If nothing hit, draw a full-length line
            Debug.DrawLine(origin, origin + direction * 100f, Color.yellow, 1f);
        }

        if(muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        lastShotTime = Time.time;
    }
    
    
}

