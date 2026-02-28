using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootForce = 20f;

    private SionUltimateSkill ultScript; // Add reference

    void Start()
    {
        ultScript = GetComponent<SionUltimateSkill>(); // Grab the component
    }

    void Update()
    {
        // Check if charging. If so, exit the update loop early.
        if (ultScript != null && ultScript.isCharging) return;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    // ... Keep your existing Shoot() method exactly the same ...
    void Shoot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 dir = (targetPoint - firePoint.position).normalized;

            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            proj.GetComponent<SionEProjectile>().Initialize(dir);
        }
    }
}