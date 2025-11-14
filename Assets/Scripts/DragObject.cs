using UnityEngine;

public class DragObject : MonoBehaviour
{
    public float planeHeight = 0f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void MoveTargetToCursor(Transform target)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            transform.position = hit.point;
        }
    }
}
