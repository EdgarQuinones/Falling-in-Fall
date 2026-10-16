
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LevelLighting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Light2D globalLight;

    [System.Serializable]
    public class LightSection
    {
        public float playerY;

        [Range(0f, 1f)]
        public float lightIntensity = 1f;
    }

    [Header("Lighting Sections")]
    [SerializeField] private LightSection[] lightSections;

    [Header("Settings")]
    [SerializeField] private float transitionSpeed = 2f;

    void Update()
    {
        if (player == null || globalLight == null)
            return;

        float targetIntensity = 1f;

        foreach (LightSection section in lightSections)
        {
            if (player.position.y >= section.playerY)
            {
                targetIntensity = section.lightIntensity;
            }
        }

        globalLight.intensity = Mathf.Lerp(
            globalLight.intensity,
            targetIntensity,
            Time.deltaTime * transitionSpeed
        );
    }
}
