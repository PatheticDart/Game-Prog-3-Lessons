using UnityEngine;

public class EnemySetup : MonoBehaviour
{
    [Header("References")]
    public GameObject breakText;
    public GameObject explosionPrefab;

    [Header("Random Movement")]
    public float moveSpeed = 2f;
    public float changeDirectionInterval = 2f;
    public float roamRadius = 8f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float changeDirTimer;
    private bool isHit = false;
    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        startPosition = transform.position;

        // Auto-find Break Text if not assigned
        if (breakText == null)
        {
            Transform t = transform.Find("Canvas/Break Text");
            if (t != null)
                breakText = t.gameObject;
        }

        if (breakText != null)
            breakText.SetActive(false);

        PickNewDirection();
    }

    void Update()
    {
        if (isHit) return;

        changeDirTimer -= Time.deltaTime;

        if (changeDirTimer <= 0f)
        {
            PickNewDirection();
        }

        // Move along XZ only
        Vector3 move = moveDirection * moveSpeed * Time.deltaTime;
        Vector3 nextPos = transform.position + move;

        // Keep within roam radius
        if (Vector3.Distance(startPosition, nextPos) > roamRadius)
        {
            PickNewDirection();
            return;
        }

        rb.MovePosition(nextPos);
    }

    void PickNewDirection()
    {
        changeDirTimer = changeDirectionInterval;

        Vector2 rand = Random.insideUnitCircle.normalized;
        moveDirection = new Vector3(rand.x, 0f, rand.y);
    }

    public void OnHitByProjectile(Vector3 hitPosition)
    {
        isHit = true;

        // Stop movement
        rb.linearVelocity = Vector3.zero;

        // Show text
        if (breakText != null)
            breakText.SetActive(true);

        // Spawn explosion
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, hitPosition, Quaternion.identity);
    }
}
