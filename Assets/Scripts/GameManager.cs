using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int maxBomb = 10;
    [SerializeField] private int bombCount;
    public float maxGameTime;
    [SerializeField] private float SceneTime;
    [SerializeField] private bool isLost = false;
    [SerializeField] private bool isWon = false;
    [SerializeField] private float cumulativConveyorOffset = 0;

    public Transform bombSpawner;
    public Transform middleConveyor;
    public Transform endConveyor;
    public Transform clockHand;
    public Animator coucouAnimator;

    public Image LoseText;
    public Image WinText;
    public TMP_Text scoreText;
    public Material conveyorMat;

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
            coucouAnimator.SetBool("isLost", true);
            TriggerLose();
        }

        if(currentBomb?.isOnConveyor == true)
        {
            conveyorMat.SetTextureOffset("_BaseMap", new Vector2(currentBomb.transform.position.x * 0.8f + 4.34f* 0.8f + cumulativConveyorOffset, 0));
            //Debug.Log(currentBomb.transform.position.x);
        }

    }

    public void StartGame()
    {
        bombCount = 0;
        SpawnNextBomb();
    }

    public void SpawnNextBomb()
    {
        bombCount++;

        if (bombCount < maxBomb)
        {

            List<Bombs> possibleBombs = new List<Bombs>();

            foreach(GameObject bomb in bombList.bombList)
            {
                if(bomb.GetComponent<Bombs>().difficulty == bombCount)
                {
                    possibleBombs.Add(bomb.GetComponent<Bombs>());

                    Debug.Log("add " + bomb.transform.name + " to possibilities with difficulty " + bomb.GetComponent<Bombs>().difficulty);
                }
            }

            if(possibleBombs.Count == 0)
            {
                Debug.LogWarning("No bomb at difficulty " + bombCount);
            }

            int rand = Random.Range(0, possibleBombs.Count);
            Debug.Log("Next bomb should be " + possibleBombs[rand]);

            currentBomb = Instantiate(possibleBombs[rand], bombSpawner).GetComponent<Bombs>();
            MoveBombToMiddle();



        }
        else if(bombCount == maxBomb) // Derniere bombe
        {
            currentBomb = Instantiate(bombList.bombList[^1], bombSpawner).GetComponent<Bombs>();
            MoveBombToMiddle();
        }
        else if(bombCount > maxBomb) // Dépassé la dernière bombe
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

            currentBomb.transform.DOMove(middleConveyor.position, 3.75f).SetEase(Ease.InOutQuad).OnComplete(() => {

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

            currentBomb.transform.DOMove(endConveyor.position, 3.75f).SetEase(Ease.InOutQuad).OnComplete(() => {

                cumulativConveyorOffset += conveyorMat.GetTextureOffset("_BaseMap").x;
                currentBomb.isMoving=false;
                soundManager.conveyorSound.Stop();

                // check si la bombe a été désamorçée
                if (currentBomb.isDefused)
                {
                    Destroy(currentBomb.gameObject);
                    currentBomb = null;
                    SpawnNextBomb();
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

            DOVirtual.DelayedCall(2, () =>
            {
                LoseText.enabled = true;
                LoseText.DOFade(1f, 2)
                   .SetEase(Ease.Linear);
            });

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
            float spareTime = Mathf.Round((maxGameTime - SceneTime) * 10.0f) * 0.1f;
            SaveScore(spareTime);

            scoreText.text = spareTime.ToString();
            scoreText.DOFade(1f, 1)
                   .SetEase(Ease.Linear);

            WinText.enabled = true;
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

        clockHand.DOLocalRotate(
            new Vector3(0, 0, 360),
            maxGameTime,
            RotateMode.WorldAxisAdd
        )
        .SetEase(Ease.Linear);
    }

    public void SaveScore(float score)
    {
        if(score > PlayerPrefs.GetFloat("score"))
        {
            PlayerPrefs.SetFloat("score", score);
        }

    }

}
