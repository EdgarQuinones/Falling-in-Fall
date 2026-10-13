
using UnityEngine;
using System.Collections;

public class LeafController : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float warningDelay = 2f;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float destroyDelay = 3f;

    [Header("Movement")]
    [SerializeField] private float shakeAmount = 0.08f;
    [SerializeField] private float fallSpeed = 5f;

    private bool isTriggered;
    private bool isFalling;
    private Vector3 startingPosition;

    private void Awake()
    {
        startingPosition = transform.position;
    }

    public void TriggerLeaf()
    {
        if (isTriggered)
            return;

        isTriggered = true;
        StartCoroutine(WarningAndFall());
    }

    private IEnumerator WarningAndFall()
    {
        // Wait before shaking.
        yield return new WaitForSeconds(warningDelay);

        // Shake side to side.
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-shakeAmount, shakeAmount);
            transform.position = startingPosition
                + new Vector3(offsetX, 0f, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Restore original position.
        transform.position = startingPosition;

        // Begin falling.
        isFalling = true;
        Destroy(gameObject, destroyDelay);
    }

    private void Update()
    {
        if (isFalling)
        {
            transform.position += Vector3.down
                * fallSpeed * Time.deltaTime;
        }
    }
}
