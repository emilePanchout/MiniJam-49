using UnityEngine;
using DG.Tweening;

public class Bombs : MonoBehaviour
{
    private Camera cam;
    private GameManager gameManager;

    public Collider collider;
    public ParticleSystem particles;
    public GameObject model;

    public int difficulty;

    public bool isDraggable = false;
    public bool isDragging = false;
    public bool isOnConveyor = true;
    public bool isMoving = false;
    public bool isDefused = false;


    public void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        cam = FindFirstObjectByType<Camera>();
    }


    void Update()
    {
        // Detection de click sans outil
        if (Input.GetMouseButtonDown(0) && gameManager.currentTool == null)
        {
            Ray r = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(r, out RaycastHit hit))
            {
                // prend la bombe
                if (hit.collider.CompareTag("Bomb") && isDraggable)
                {
                    isDragging = true;
                    isOnConveyor = false;
                    collider.enabled = false;
                }

                // pose la bombe sur la table
                if (hit.collider.CompareTag("TableArea") && isDragging)
                {
                    isDragging = false;
                    collider.enabled = true;
                    gameObject.transform.position = hit.collider.transform.position;
                }

                // repose la bombe sur le tapis
                if (hit.collider.CompareTag("ConveyorArea") && isDragging)
                {
                    isDragging = false;
                    isOnConveyor = true;
                    collider.enabled = true;
                    gameObject.transform.position = hit.collider.transform.position;
                }

            }
        }

        // Detection de click avec outil
        if (Input.GetMouseButtonDown(0) && gameManager.currentTool != null && !isOnConveyor)
        {
            Ray r = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(r, out RaycastHit hit))
            {
                // applique l'outil
                if (hit.collider.CompareTag("Bomb") && isDraggable)
                {
                    TryDefuse(gameManager.currentTool.toolName);
                }
            }
        }

        // L'objet suit la souris
        if (isDragging)
        {
            GetComponent<DragObject>().MoveTargetToCursor(gameObject.transform);
        }
    }

    public virtual void TryDefuse(string toolName) { }

    public void TriggerExplosion()
    {
        particles.Play();
        Destroy(model);

        DOVirtual.DelayedCall(particles.main.duration, () =>
        {
            gameManager.TriggerLose();
        });

    }


}
