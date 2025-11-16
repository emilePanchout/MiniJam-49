using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Bombs : MonoBehaviour
{
    private Camera cam;
    protected GameManager gameManager;
    private SoundManager SoundManager;
    private Collider areaCollider;

    public Collider collider;

    public ParticleSystem particles;
    public GameObject model;
    public AudioSource audioSource;

    public int difficulty;

    public bool isDraggable = false;
    public bool isDragging = false;
    public bool isOnConveyor = true;
    public bool isMoving = false;
    public bool isDefused = false;
    public bool isUnpacked = true;
    public bool isInHand = false;


    public List<string> defusersList;
    public List<string> exploderList;

    public void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        SoundManager = FindFirstObjectByType<SoundManager>();
        areaCollider = GameObject.Find("Area").GetComponent<Collider>();
        cam = FindFirstObjectByType<Camera>();
    }


    public void Update()
    {

        // Detection de click sans outil
        if (Input.GetMouseButtonDown(0) && gameManager.currentTool == null)
        {
            Ray r = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(r, out RaycastHit hit))
            {
                // prend la bombe du tapis
                if (hit.collider.CompareTag("Bomb") && isDraggable && isOnConveyor)
                {
                    isDragging = true;
                    isOnConveyor = false;
                    collider.enabled = false;
                    isInHand = true;

                    gameManager.handCursor.SwitchHands();
                }

                // prend la bombe unpacked
                else if (hit.collider.CompareTag("Bomb") && isDraggable && !isOnConveyor && isUnpacked)
                {
                    isDragging = true;
                    collider.enabled = false;
                    areaCollider.enabled = true;
                    isInHand = true;

                    gameManager.handCursor.SwitchHands();
                }

                // unpack la bombe
                else if (hit.collider.CompareTag("Bomb") && isDraggable && !isOnConveyor && !isUnpacked)
                {
                    Unpack();
                }

                // Defuse sans outil
                else if (hit.collider.CompareTag("Bomb") && !isOnConveyor && isUnpacked)
                {
                    TryDefuse("Hands");
                }

                // pose la bombe sur la table
                if (hit.collider.CompareTag("TableArea") && isDragging)
                {
                    isDragging = false;
                    collider.enabled = true;
                    areaCollider.enabled = false;
                    isInHand = false;

                    gameObject.transform.position = hit.collider.transform.position;

                    gameManager.handCursor.SwitchHands();

                    PutOnTable();
                }

                // repose la bombe sur le tapis
                if (hit.collider.CompareTag("ConveyorArea") && isDragging && isUnpacked)
                {
                    isDragging = false;
                    isOnConveyor = true;
                    collider.enabled = true;
                    isInHand = false;
                    gameObject.transform.position = hit.collider.transform.position;

                    gameManager.handCursor.SwitchHands();
                }

            }
        }

        // Detection de click avec outil sur bomb
        if (Input.GetMouseButtonDown(0) && gameManager.currentTool != null && !isOnConveyor && isUnpacked)
        {
            Ray r = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(r, out RaycastHit hit))
            {
                // applique l'outil
                if (hit.collider.CompareTag("Bomb"))
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
    public virtual void Unpack() { }

    public virtual void PutOnTable() { }

    public void TriggerExplosion()
    {
        particles.Play();
        SoundManager.ExplosionSound.Play();
        Destroy(model);

        DOVirtual.DelayedCall(particles.main.duration, () =>
        {
            gameManager.TriggerLose();
        });

    }

    public void TriggerExplosion(AudioSource sound, float time)
    {
        particles.Play();
        sound.time = time;
        sound.Play();
        Destroy(model);

        DOVirtual.DelayedCall(particles.main.duration, () =>
        {
            gameManager.TriggerLose();
        });

    }


}
