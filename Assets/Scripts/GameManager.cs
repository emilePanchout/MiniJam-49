using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int bombCount;
    public int maxBomb = 10;

    public Transform bombSpawner;
    public Transform middleConveyor;
    public Transform endConveyor;

    public TMP_Text LoseText;

    public BombList bombList;

    public Bombs currentBomb;
    public Tools currentTool;

    public void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        bombCount = 1;
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

        }
        
    }

    public void MoveBombToMiddle()
    {
        if (currentBomb.isOnConveyor && !currentBomb.isMoving)
        {
            currentBomb.isMoving = true;

            currentBomb.transform.DOMove(middleConveyor.position, 3.75f).OnComplete(() => {
                currentBomb.isDraggable = true;
                currentBomb.isMoving = false;
            });
        }
    }

    public void MoveBombToEnd()
    {
        if (currentBomb.isOnConveyor && !currentBomb.isMoving)
        {
            currentBomb.isDraggable = false;
            currentBomb.isMoving = true;

            currentBomb.transform.DOMove(endConveyor.position, 3.75f).OnComplete(() => {
                currentBomb.isMoving=false;

                // check si la bombe a été désamorçée
                if(currentBomb.isDefused)
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
        Debug.Log("You won with " + "time left");
    }
}
