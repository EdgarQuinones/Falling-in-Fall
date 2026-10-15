
using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    [SerializeField] private GameObject flashlight;

    public bool isOn { get; private set; }

    private Camera mainCamera;

    [SerializeField] private float lightRange = 6f;
    [SerializeField] private float beamAngle = 45f;

    public bool IsPositionIlluminated(Vector2 position)
    {
        if (!isOn)
            return false;

        Vector2 directionToPosition =
            position - (Vector2)flashlight.transform.position;

        if (directionToPosition.magnitude > lightRange)
            return false;

        float angle = Vector2.Angle(
            flashlight.transform.up,
            directionToPosition
        );

        return angle <= beamAngle / 2f;
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        flashlight.SetActive(false);
    }

    private void Update()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(mousePosition.x, mousePosition.y,
                -mainCamera.transform.position.z));

        Vector2 direction = worldPosition - flashlight.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        flashlight.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    public void OnFlash(InputValue value)
    {
        if (!value.isPressed)
            return;

        isOn = !isOn;
        flashlight.SetActive(isOn);
    }


}
