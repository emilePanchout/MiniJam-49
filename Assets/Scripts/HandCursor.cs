using UnityEngine;

public class HandCursor : MonoBehaviour
{
    [Header("References")]
    public GameObject handParent;
    public GameObject releasedHand;
    public GameObject closedHand;


    [Header("Canvas et caméra (optionnel)")]
    private Canvas canvas;
    private Camera cam;

    void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>().GetComponent<Canvas>();

        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    void Update()
    {
        if (handParent == null || canvas == null) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out localPoint
        );

        handParent.transform.localPosition = localPoint;
    }

    public void SwitchHands()
    {
        if(releasedHand.activeSelf)
        {
            releasedHand.SetActive(false);
            closedHand.SetActive(true);
        }
        else
        {
            closedHand.SetActive(false);
            releasedHand.SetActive(true);
        }
    }
}
