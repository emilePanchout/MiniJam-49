using UnityEngine;

public class Tools : MonoBehaviour
{
    public string toolName;

    public Collider collider;
    public Transform root;

    public bool isDraggable = false;
    public bool isDragging = false;

    private Camera cam;
    private GameManager gameManager;

    public void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>().GetComponent<GameManager>();
        cam = FindFirstObjectByType<Camera>();
    }

    public void Update()
    {
        // Detection de click gauche
        if (Input.GetMouseButtonDown(0) && gameManager.currentTool == null)
        {
            Ray r = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(r, out RaycastHit hit))
            {
                // prend sur l'objet
                if (hit.collider.CompareTag("Tool") && hit.collider.gameObject == gameObject && isDraggable)
                {
                    isDragging = true;
                    collider.enabled = false;

                    gameManager.currentTool = hit.collider.gameObject.GetComponent<Tools>();
                    gameManager.handCursor.SwitchHands();
                }

            }
        }

        // L'objet suit la souris
        if (isDragging)
        {
            GetComponent<DragObject>().MoveTargetToCursor(gameObject.transform);
        }

        // Detection de click droit
        if (Input.GetMouseButtonDown(1) && gameManager.currentTool == this)
        {
            isDragging = false;
            collider.enabled = true;
            gameManager.currentTool = null;
            transform.position = root.position;

            gameManager.handCursor.SwitchHands();
        }
    }
}
