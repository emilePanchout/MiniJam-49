using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFitUICanvas : MonoBehaviour
{
    public Canvas canvas;           // Canvas qui contient la zone UI
    public SpriteRenderer spriteRenderer;
    private Vector2 lastScreenSize;

    void Start()
    {
        FitSprite();
    }

    void Update()
    {
        Vector2 currentSize = new Vector2(Screen.width, Screen.height);
        if (currentSize != lastScreenSize)
        {
            FitSprite();
            lastScreenSize = currentSize;
        }
    }

    void FitSprite()
    {
        if (canvas == null || spriteRenderer.sprite == null) return;

        Camera cam = canvas.worldCamera;
        if (cam == null) cam = Camera.main;

        // Taille du Canvas en pixels
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        // Conversion pixel ? unité monde
        float worldHeight = canvasHeight / (canvas.pixelRect.height / (cam.orthographicSize * 2f));
        float worldWidth = canvasWidth / (canvas.pixelRect.width / (cam.orthographicSize * 2f) * cam.aspect);

        // Taille du sprite en unité monde
        float spriteW = spriteRenderer.sprite.bounds.size.x;
        float spriteH = spriteRenderer.sprite.bounds.size.y;

        // Scale pour couvrir
        float scaleX = worldWidth / spriteW;
        float scaleY = worldHeight / spriteH;
        float finalScale = Mathf.Max(scaleX, scaleY);

        spriteRenderer.transform.localScale = new Vector3(finalScale, finalScale, 1f);

        // Centre sur le Canvas
        Vector3 worldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, canvasRect.position, cam, out worldPos);
        spriteRenderer.transform.position = worldPos;
    }
}
