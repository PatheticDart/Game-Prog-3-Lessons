using UnityEngine;

[RequireComponent(typeof(SionUltimateSkill))]
public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float rotationSpeed = 720f;

    private SionUltimateSkill ultScript;

    void Start()
    {
        ultScript = GetComponent<SionUltimateSkill>();
    }

    void Update()
    {
        // Prevent normal movement if the ultimate is active
        if (ultScript != null && ultScript.isCharging) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 moveDir = new Vector3(h, 0f, v).normalized;

        if (moveDir.magnitude >= 0.1f)
        {
            // Move the player
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

            // Rotate the player to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}