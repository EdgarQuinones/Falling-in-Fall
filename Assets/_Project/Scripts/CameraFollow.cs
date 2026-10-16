
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [System.Serializable]
    public class CameraSection
    {
        public float playerY;
        public float cameraY;
    }

    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Settings")]
    [SerializeField] private float smoothSpeed = 2f;
    [SerializeField] private float fallBuffer = 3f;

    [Header("Camera Sections")]
    [SerializeField] private CameraSection[] sections;

    private float initialCameraY;
    private int currentSection = -1;

    private void Start()
    {
        initialCameraY = transform.position.y;
    }

    private void LateUpdate()
    {
        if (player == null || sections == null || sections.Length == 0)
            return;

        // Move up when reaching a new section.
        while (currentSection + 1 < sections.Length &&
               player.position.y >= sections[currentSection + 1].playerY)
        {
            currentSection++;
        }

        // Move down only after falling below the buffer.
        while (currentSection >= 0 &&
               player.position.y <
               sections[currentSection].playerY - fallBuffer)
        {
            currentSection--;
        }

        float targetY = currentSection >= 0
            ? sections[currentSection].cameraY
            : initialCameraY;

        float newY = Mathf.Lerp(
            transform.position.y,
            targetY,
            smoothSpeed * Time.deltaTime
        );

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }
}
