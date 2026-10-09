using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpeed = 5f;

    [SerializeField] private float yOffset;
    [SerializeField] private float playerOffset = 5f;

    void Start()
    {
        yOffset = transform.position.y - player.position.y;
    }

    void LateUpdate()
    {
        float targetY = (player.position.y - playerOffset) + yOffset;

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
