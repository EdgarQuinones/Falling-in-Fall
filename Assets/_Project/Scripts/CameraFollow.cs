
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [System.Serializable]
    public class CameraSection
    {
        public float playerY;
        public float cameraY;
    }

    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private CameraSection[] sections;

    private float initialCameraY;

    void Start()
    {
        initialCameraY = transform.position.y;
    }

    void LateUpdate()
    {
        float targetY = initialCameraY;

        // Find the highest section the player has reached.
        foreach (CameraSection section in sections)
        {
            if (player.position.y >= section.playerY)
            {
                targetY = section.cameraY;
            }
        }

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
