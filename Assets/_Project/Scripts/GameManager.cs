using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FlashlightController flashlightController;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D globalLight;    // Update is called once per frame
    void Update()
    {
        if (flashlightController.isOn)
        {
            globalLight.intensity = 0f;
        }
        else
        {
            globalLight.intensity = 0.01f;
        }
    }
}
