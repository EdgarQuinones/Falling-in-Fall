using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireflyController : MonoBehaviour
{
    [SerializeField] private FlashlightController flashlightController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D fireflyLight;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fireflyLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    private void Update()
    {
        bool visible = !flashlightController.isOn;

        spriteRenderer.enabled = visible;
        fireflyLight.enabled = visible;
    }
}
