using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int maxBomb = 10;
    [SerializeField] private int bombCount;
    public float maxGameTime = 100;
    [SerializeField] private float SceneTime;
    [SerializeField] private bool isLost = false;
    [SerializeField] private bool isWon = false;

    public Transform bombSpawner;
    public Transform middleConveyor;
    public Transform endConveyor;
    public Transform clockHand;

    public TMP_Text LoseText;
    public TMP_Text WinText;

    public SoundManager soundManager;
    public HandCursor handCursor;

    public BombList bombList;

    public Bombs currentBomb;
    public Tools currentTool;

    public void Start()
    {
        StartGame();
        StartClockRotation();
       

        Cursor.visible = false;
    }

    private void Update()
    {
        // mesure le temps
        SceneTime = Time.timeSinceLevelLoad;

        if (SceneTime > maxGameTime && !isLost)
        {
            TriggerLose();
        }
    }

    public void StartGame()
    {
        bombCount = 0;
        SpawnNextBomb(1);
    }

    public void SpawnNextBomb(int maxDifficulty)
    {
        if(bombCount < maxBomb)
        {
            bombCount++;

            int rand = Random.Range(0, bombList.bombList.Count);
            if (bombList.bombList[rand].GetComponent<Bombs>().difficulty <= maxDifficulty)
            {
                currentBomb = Instantiate(bombList.bombList[rand], bombSpawner).GetComponent<Bombs>();
                MoveBombToMiddle();
            }
        }
        else
        {
            TriggerVictory();
        }
        
    }

    public void MoveBombToMiddle()
    {
        if (currentBomb.isOnConveyor && !currentBomb.isMoving)
        {
            currentBomb.isMoving = true;
            soundManager.conveyorSound.Play();

            currentBomb.transform.DOMove(middleConveyor.position, 3.75f).OnComplete(() => {

                currentBomb.isDraggable = true;
                currentBomb.isMoving = false;
                soundManager.conveyorSound.Stop();
            });
        }
    }

    public void MoveBombToEnd()
    {
        if (currentBomb.isOnConveyor && !currentBomb.isMoving)
        {
            currentBomb.isDraggable = false;
            currentBomb.isMoving = true;
            soundManager.conveyorSound.Play();

            currentBomb.transform.DOMove(endConveyor.position, 3.75f).OnComplete(() => {

                currentBomb.isMoving=false;
                soundManager.conveyorSound.Stop();

                // check si la bombe a été désamorçée
                if (currentBomb.isDefused)
                {
                    Destroy(currentBomb.gameObject);
                    SpawnNextBomb(bombCount);
                }
                else
                {
                    currentBomb.TriggerExplosion();
                }

            });
        }
        
    }

    public void TriggerLose()
    {
        if(!isLost && !isWon)
        {
            isLost = true;

            LoseText.DOFade(1f, 1)
               .SetEase(Ease.Linear);

            DOVirtual.DelayedCall(3, () =>
            {
                SceneManager.LoadScene("GameScene");
            });
        }
        
    }

    public void TriggerVictory()
    {
        if(!isWon && !isLost)
        {
            isWon = true;
            WinText.text = "You won with " + Mathf.Round((maxGameTime - SceneTime) * 10.0f) * 0.1f + " seconds of spare time";

            WinText.DOFade(1f, 1)
                   .SetEase(Ease.Linear);

            DOVirtual.DelayedCall(3, () =>
            {
                SceneManager.LoadScene("GameScene");
            });
        }
    }

    public void StartClockRotation()
    {
        // Reset au début
        clockHand.localRotation = Quaternion.Euler(0, 0, 0);

        clockHand.DOLocalRotate(
            new Vector3(0, 0, -360),  // -360° sens horaire
            10,
            RotateMode.FastBeyond360
        )
        .SetEase(Ease.Linear);
    }
}
