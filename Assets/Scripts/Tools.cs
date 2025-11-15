using UnityEngine;

public class Tools : MonoBehaviour
{
    public string toolName;

    public Collider collider;
    public Transform root;

    public bool isDraggable = false;
    public bool isDragging = false;

    private Camera cam;
    protected GameManager gameManager;

    public void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>().GetComponent<GameManager>();
        cam = FindFirstObjectByType<Camera>();
    }

    public void Update()
    {
        // Detection de click gauche sans outil
        if (Input.GetMouseButtonDown(0) && gameManager.currentTool == null)
        {
            Ray r = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(r, out RaycastHit hit))
            {
                // prend l'outil
                if (hit.collider.CompareTag("Tool") && hit.collider.gameObject == gameObject && isDraggable &&
                    !gameManager.currentBomb.isInHand && ((gameManager.currentBomb.isUnpacked && !gameManager.currentBomb.isOnConveyor) || (!gameManager.currentBomb.isUnpacked && gameManager.currentBomb.isOnConveyor)))
                {
                    isDragging = true;
                    collider.enabled = false;
                    TakeTool();

                    gameManager.currentTool = hit.collider.gameObject.GetComponent<Tools>();
                    gameManager.handCursor.SwitchHands();
                }

            }
        }
        // Detection de click gauche avec outil
        else if (Input.GetMouseButtonDown(0) && gameManager.currentTool != null && !gameManager.currentBomb.isInHand)
        {
            ActivateTool();
        }
        else if (Input.GetMouseButtonUp(0) && gameManager.currentTool != null && !gameManager.currentBomb.isInHand)
        {
            DeactivateTool();
        }

        // Detection de click droit
        if (Input.GetMouseButtonDown(1) && gameManager.currentTool == this && !gameManager.currentBomb.isInHand)
        {
            isDragging = false;
            collider.enabled = true;
            gameManager.currentTool = null;
            transform.position = root.position;
            DropTool();

            gameManager.handCursor.SwitchHands();
        }

        // L'objet suit la souris
        if (isDragging)
        {
            GetComponent<DragObject>().MoveTargetToCursor(gameObject.transform);
        }
    }

    public virtual void TakeTool()
    {

    }

    public virtual void DropTool()
    {

    }

    public virtual void ActivateTool()
    { 
    
    }

    public virtual void DeactivateTool()
    {

    }
}
