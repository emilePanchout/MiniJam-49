using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int bombCount;
    public int maxBomb = 10;
    public float maxGameTime = 100;

    public Transform bombSpawner;
    public Transform middleConveyor;
    public Transform endConveyor;

    public TMP_Text LoseText;
    public TMP_Text WinText;

    public TimeManager timeManager;
    public SoundManager soundManager;
    public HandCursor handCursor;

    public BombList bombList;

    public Bombs currentBomb;
    public Tools currentTool;

    public void Start()
    {
        StartGame();
        Cursor.visible = false;
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
        LoseText.DOFade(1f, 1)
               .SetEase(Ease.Linear);

        DOVirtual.DelayedCall(3, () =>
        {
            SceneManager.LoadScene("GameScene");
        });
        
    }

    public void TriggerVictory()
    {
        WinText.text = "You won with " + Mathf.Round((maxGameTime - timeManager.GetTimeScene()) * 10.0f) * 0.1f + " seconds of spare time";

        WinText.DOFade(1f, 1)
               .SetEase(Ease.Linear);

        DOVirtual.DelayedCall(3, () =>
        {
            SceneManager.LoadScene("GameScene");
        });
    }
}
