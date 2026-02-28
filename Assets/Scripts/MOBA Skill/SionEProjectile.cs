using UnityEngine;

public class SionEProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float maxDistance = 12f;

    private Vector3 direction;
    private Vector3 startPos;

    private Transform grabbedEnemy;

    public void Initialize(Vector3 dir)
    {
        direction = dir;
        startPos = transform.position;
        transform.forward = dir;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (grabbedEnemy != null)
        {
            grabbedEnemy.position = transform.position;
        }

        float traveled = Vector3.Distance(startPos, transform.position);
        if (traveled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (grabbedEnemy != null) return;

        if (other.CompareTag("Enemy"))
        {
            grabbedEnemy = other.transform;

            // Notify enemy it got hit
            EnemySetup enemy = other.GetComponent<EnemySetup>();
            if (enemy != null)
            {
                enemy.OnHitByProjectile(other.ClosestPoint(transform.position));
            }

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }
    }

    void OnDestroy()
    {
        if (grabbedEnemy != null)
        {
            Rigidbody rb = grabbedEnemy.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
    }
}