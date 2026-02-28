using UnityEngine;

public class EnemySetup : MonoBehaviour
{
    [Header("References")]
    public GameObject breakText;
    public GameObject stunText;
    public GameObject explosionPrefab;

    [Header("Random Movement")]
    public float moveSpeed = 2f;
    public float changeDirectionInterval = 2f;
    public float roamRadius = 8f;

    [Header("Status Durations")]
    public float breakDuration = 1.5f; // NEW: How long the E skill disables the enemy

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float changeDirTimer;
    public bool isHit = false;
    private Vector3 startPosition;
    private Coroutine statusCoroutine; // Renamed to handle BOTH stun and break states

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        startPosition = transform.position;

        // Auto-find Break Text
        if (breakText == null)
        {
            Transform t = transform.Find("Canvas/Break Text");
            if (t != null) breakText = t.gameObject;
        }
        if (breakText != null) breakText.SetActive(false);

        // Auto-find Stun Text
        if (stunText == null)
        {
            Transform t = transform.Find("Canvas/Stun Text");
            if (t != null) stunText = t.gameObject;
        }
        if (stunText != null) stunText.SetActive(false);

        PickNewDirection();
    }

    void Update()
    {
        // Only handle timers and input in Update
        if (isHit) return;

        changeDirTimer -= Time.deltaTime;
        if (changeDirTimer <= 0f) PickNewDirection();
    }

    void FixedUpdate()
    {
        // Handle all Rigidbody movement in FixedUpdate
        if (isHit) return;

        // Note the use of Time.fixedDeltaTime instead of Time.deltaTime
        Vector3 move = moveDirection * moveSpeed * Time.fixedDeltaTime;
        Vector3 nextPos = rb.position + move; // Use rb.position for physics calculations

        // Optimization: sqrMagnitude is significantly faster than Vector3.Distance
        if ((startPosition - nextPos).sqrMagnitude > roamRadius * roamRadius)
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

    // UPDATED: E Skill Reaction (Now temporary)
    public void OnHitByProjectile(Vector3 hitPosition)
    {
        if (isHit) return; // Prevent double-hits

        isHit = true;
        rb.linearVelocity = Vector3.zero;

        if (breakText != null) breakText.SetActive(true);
        if (explosionPrefab != null) Instantiate(explosionPrefab, hitPosition, Quaternion.identity);

        // Start the timer to remove the break state
        if (statusCoroutine != null) StopCoroutine(statusCoroutine);
        statusCoroutine = StartCoroutine(StatusRoutine(breakDuration, breakText));
    }

    // Ultimate Skill Reaction
    public void OnHitByUltimate(float knockupForce, float stunDuration)
    {
        if (isHit) return;

        isHit = true;
        rb.linearVelocity = Vector3.zero;

        // Apply the knockup force
        rb.AddForce(Vector3.up * knockupForce, ForceMode.Impulse);

        // Activate the new text UI
        if (stunText != null) stunText.SetActive(true);

        // Start the timer to remove the stun state
        if (statusCoroutine != null) StopCoroutine(statusCoroutine);
        statusCoroutine = StartCoroutine(StatusRoutine(stunDuration, stunText));
    }

    // NEW: Unified Status Timer Logic
    // This coroutine now takes the specific UI element so it knows which text to turn off
    private System.Collections.IEnumerator StatusRoutine(float duration, GameObject activeUI)
    {
        yield return new WaitForSeconds(duration);

        // End of status: reset everything so they can move again
        isHit = false;
        if (activeUI != null) activeUI.SetActive(false);
        PickNewDirection();
    }
}