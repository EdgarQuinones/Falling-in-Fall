using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    [SerializeField] private GameObject flashlight;

    private bool isOn;

    private void Awake()
    {
        flashlight.SetActive(false);
    }

    public void OnFlash(InputValue value)
    {
        if (!value.isPressed)
            return;

        isOn = !isOn;
        flashlight.SetActive(isOn);
    }
}
