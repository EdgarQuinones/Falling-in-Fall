
using System.Collections.Generic;
using UnityEngine;

public class Swappable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FlashlightController flashlightController;
    [SerializeField] private Transform[] positions;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private Collider2D platformCollider;

    [Header("Settings")]
    [SerializeField] private float unobservedTime = 1f;
    [SerializeField] private float shakeAmount = 0.025f;
    [SerializeField] private float shakeSpeed = 12f;

    private bool playerColliding = false;

    // Prevent swapping more than once per state.
    private bool hasSwappedWhileOff;
    private bool wasOccupiedWhenLightTurnedOff;
    private bool previousLightState;
    private bool hasSwappedWhileUnilluminated;

    private float timer;
    private int currentIndex;
    private Vector3 originalPosition;

    private void Start()
    {
        if (positions == null || positions.Length == 0)
        {
            Debug.LogWarning($"{name}: No positions assigned.");
            enabled = false;
            return;
        }

        transform.position = positions[0].position;
        currentIndex = 0;
        originalPosition = transform.position;
    }

    private void Update()
    {
        bool lightIsOn = flashlightController.isOn;

        bool illuminated = lightIsOn &&
            flashlightController.IsPositionIlluminated(transform.position);

        // Detect flashlight state changes.
        if (lightIsOn != previousLightState)
        {
            if (!lightIsOn)
            {
                wasOccupiedWhenLightTurnedOff = playerColliding;
                hasSwappedWhileOff = false;
            }

            previousLightState = lightIsOn;
        }

        // Shake when illuminated, unless the player is standing on it.
        if (illuminated && !playerColliding)
        {
            float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            transform.position = originalPosition +
                new Vector3(offsetX, 0f, 0f);
        }
        else
        {
            transform.position = originalPosition;
        }

        // Don't swap while the player is on the platform.
        if (playerColliding)
        {
            timer = 0f;
            return;
        }

        // Flashlight OFF: swap instantly, at most once.
        if (!lightIsOn)
        {
            if (wasOccupiedWhenLightTurnedOff || hasSwappedWhileOff)
                return;

            TrySwap();
            hasSwappedWhileOff = true;
            timer = 0f;
            return;
        }

        // Platform is illuminated.
        if (illuminated)
        {
            timer = 0f;
            hasSwappedWhileUnilluminated = false;
            return;
        }

        // Platform is unilluminated.
        if (hasSwappedWhileUnilluminated)
            return;

        timer += Time.deltaTime;

        if (timer >= unobservedTime)
        {
            TrySwap();
            hasSwappedWhileUnilluminated = true;
            timer = 0f;
        }
    }

    private void TrySwap()
    {
        List<int> candidates = new List<int>();

        // Find valid, unilluminated destinations.
        for (int i = 0; i < positions.Length; i++)
        {
            if (i == currentIndex || positions[i] == null)
                continue;

            if (!flashlightController.IsPositionIlluminated(
                positions[i].position))
            {
                candidates.Add(i);
            }
        }

        // No safe destination? Keep the current position.
        if (candidates.Count == 0)
            return;

        int choice = candidates[Random.Range(0, candidates.Count)];

        transform.position = positions[choice].position;
        currentIndex = choice;
        originalPosition = transform.position;
        timer = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerColliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerColliding = false;
    }
}
