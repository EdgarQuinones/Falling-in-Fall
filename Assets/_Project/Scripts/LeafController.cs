
using UnityEngine;
using System.Collections;

public class LeafController : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float warningDelay = 2f;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float respawnDelay = 7f;

    [Header("Movement")]
    [SerializeField] private float shakeAmount = 0.08f;

    private bool isTriggered;
    private Vector3 startingPosition;

    private Collider2D solidCollider;
    private Collider2D triggerCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        startingPosition = transform.position;

        // Solid collider on the leaf itself.
        solidCollider = GetComponent<Collider2D>();

        // Trigger collider on the child TriggerZone.
        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            if (col.isTrigger)
            {
                triggerCollider = col;
                break;
            }
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TriggerLeaf()
    {
        if (isTriggered)
            return;

        isTriggered = true;
        StartCoroutine(CrumbleAndRespawn());
    }

    private IEnumerator CrumbleAndRespawn()
    {
        // Wait before the leaf starts shaking.
        yield return new WaitForSeconds(warningDelay);

        // Shake the leaf.
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-shakeAmount, shakeAmount);

            transform.position = startingPosition
                + new Vector3(offsetX, 0f, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Hide the leaf and disable collisions.
        transform.position = startingPosition;
        spriteRenderer.enabled = false;
        solidCollider.enabled = false;

        if (triggerCollider != null)
            triggerCollider.enabled = false;

        // Wait before bringing the leaf back.
        yield return new WaitForSeconds(respawnDelay);

        // Restore the leaf.
        transform.position = startingPosition;
        spriteRenderer.enabled = true;
        solidCollider.enabled = true;

        if (triggerCollider != null)
            triggerCollider.enabled = true;

        isTriggered = false;
    }
}
