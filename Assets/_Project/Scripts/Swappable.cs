
using System.Collections.Generic;
using UnityEngine;

public class Swappable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FlashlightController flashlightController;
    [SerializeField] private Transform[] positions;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private Collider2D platformCollider;

    // So it only swaps once if flashlight is off
    private bool hasSwappedWhileOff;

    private bool playerColliding = false;

    private bool wasOccupiedWhenLightTurnedOff;
    private bool previousLightState;

    [Header("Settings")]
    [SerializeField] private float unobservedTime = 1f;

    private float timer;
    private int currentIndex;

    private void Start()
    {
        if (positions == null || positions.Length == 0)
        {
            Debug.LogWarning($"{name}: No positions assigned.");
            enabled = false;
            return;
        }

        transform.position = positions[0].position;

        // Identify which marker matches the object's starting position.
        float closestDistance = float.MaxValue;

        for (int i = 0; i < positions.Length; i++)
        {
            if (positions[i] == null)
                continue;

            float distance = Vector2.Distance(
                transform.position, positions[i].position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentIndex = i;
            }
        }
    }

    private void Update()
    {
        bool lightIsOn = flashlightController.isOn;

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

        // Flashlight ON and platform illuminated: reset timer.
        if (flashlightController.IsPositionIlluminated(transform.position))
        {
            timer = 0f;
            return;
        }

        // Flashlight ON but pointed away: wait before swapping.
        timer += Time.deltaTime;

        if (timer >= unobservedTime)
        {
            TrySwap();
            timer = 0f;
        }
    }

    private void TrySwap()
    {
        List<int> candidates = new List<int>();

        // Find all valid, unilluminated destinations.
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

        // No safe destination? Keep waiting and try again next frame.
        if (candidates.Count == 0)
            return;

        int choice = candidates[Random.Range(0, candidates.Count)];

        transform.position = positions[choice].position;
        currentIndex = choice;
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
