using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int maxBomb = 10;
    [SerializeField] private int bombCount;
    public float maxGameTime;
    [SerializeField] private float SceneTime;
    [SerializeField] private bool isLost = false;
    [SerializeField] private bool isWon = false;

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

        if(currentBomb.isOnConveyor)
        {
            conveyorMat.SetTextureOffset("_BaseMap", new Vector2(currentBomb.transform.position.x * 0.8f + 0.8f, 0));
        }

    }

    public void StartGame()
    {
        bombCount = 0;
        SpawnNextBomb(1);
        MoveConveyor();
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
        Debug.Log("Move");
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

            LoseText.DOFade(1f, 2)
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
            float spareTime = Mathf.Round((maxGameTime - SceneTime) * 10.0f) * 0.1f;
            SaveScore(spareTime);

            scoreText.text = spareTime.ToString();
            scoreText.DOFade(1f, 1)
                   .SetEase(Ease.Linear);

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

    public void MoveConveyor()
    {
        //conveyorMat.DOOffset(new Vector2(100,0), 160);
    }
    public void StopConveyor()
    {

    }
}
