using UnityEngine;

public class SionUltimateSkill : MonoBehaviour
{
    [Header("Models")]
    public GameObject normalModel; // Assign your normal capsule here
    public GameObject ultModel;    // Assign your horizontal rectangle here

    [Header("Ultimate Stats")]
    public float startSpeed = 10f;
    public float maxSpeed = 25f;
    public float acceleration = 5f;
    public float turnSpeed = 45f;

    [Header("Impact Stats")]
    public float knockupForce = 15f;
    public float stunDuration = 1.5f;

    public bool isCharging { get; private set; }
    private float currentSpeed;

    void Start()
    {
        // Ensure we start in the correct visual state
        normalModel.SetActive(true);
        ultModel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isCharging) StartCharge();
            else StopCharge(); // Allow manual cancel by pressing R again
        }

        if (isCharging)
        {
            HandleChargeMovement();
        }
    }

    void StartCharge()
    {
        isCharging = true;
        currentSpeed = startSpeed;
        normalModel.SetActive(false);
        ultModel.SetActive(true);
    }

    void HandleChargeMovement()
    {
        // Auto-move forward
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime, Space.Self);

        // Limited steering with A/D or Left/Right arrows
        float steer = Input.GetAxis("Horizontal");
        transform.Rotate(0f, steer * turnSpeed * Time.deltaTime, 0f);

        // Gradually increase speed
        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
    }

    public void StopCharge()
    {
        isCharging = false;
        normalModel.SetActive(true);
        ultModel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isCharging) return;

        // Check which type of enemy we hit
        bool isHero = other.CompareTag("Enemy");
        bool isMinion = other.CompareTag("MinionEnemy");

        if (isHero || isMinion)
        {
            EnemySetup enemy = other.GetComponent<EnemySetup>();

            // Only apply if the target isn't already hit/stunned
            if (enemy != null && !enemy.isHit)
            {
                // Apply the exact same reaction (knockup + stun text) for both
                enemy.OnHitByUltimate(knockupForce, stunDuration);

                // ONLY stop the charge if it is a normal Enemy hero
                if (isHero)
                {
                    StopCharge();
                }
                // If it's a MinionEnemy, the code skips StopCharge() and keeps going!
            }
        }
    }
}