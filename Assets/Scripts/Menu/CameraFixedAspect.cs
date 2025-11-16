using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFixedAspect : MonoBehaviour
{
    [Tooltip("Target aspect ratio (width / height)")]
    public float targetAspect = 16f / 9f;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        UpdateCamera();
    }

    void Update()
    {
        // Optionnel : recalculer si la fenêtre change
        UpdateCamera();
    }

    void UpdateCamera()
    {
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1f)
        {
            // bandes horizontales (letterbox)
            cam.rect = new Rect(0, (1f - scaleHeight) / 2f, 1f, scaleHeight);
        }
        else
        {
            // bandes verticales (pillarbox)
            float scaleWidth = 1f / scaleHeight;
            cam.rect = new Rect((1f - scaleWidth) / 2f, 0, scaleWidth, 1f);
        }
    }
}
