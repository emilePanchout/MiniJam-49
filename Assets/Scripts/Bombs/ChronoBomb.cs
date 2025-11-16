using UnityEngine;
using DG.Tweening;

public class ChronoBomb : Bombs
{
    public int maxBombTimer = 20;
    [SerializeField] private float elapsedTime = 0f;
    [SerializeField] private bool isRunning = true;

    [Header("References")]
    public GameObject packageSprite;
    public GameObject tntSprite;
    public GameObject smashedSprite;
    public GameObject timeSprite;

    public int maxForce;
    public int minForce;

    void Start()
    {
        elapsedTime = 0f;
        MoveTimer();
    }

    void Update()
    {
        base.Update();

        if (isRunning && elapsedTime < maxBombTimer)
        {
            elapsedTime += Time.deltaTime;
        }
        else if (elapsedTime > maxBombTimer && isRunning)
        {
            isRunning = false;
            TriggerExplosion();
        }
    }

    public override void Unpack()
    {
        int rndx = Random.Range(minForce, maxForce);
        int rndy = Random.Range(minForce, maxForce);
        packageSprite.GetComponent<Rigidbody>().useGravity = true;
        packageSprite.GetComponent<Rigidbody>().AddForce(Vector3.up * 2f * rndx, ForceMode.Impulse);
        packageSprite.GetComponent<Rigidbody>().AddForce(Vector3.right * 2f * rndy, ForceMode.Impulse);
        packageSprite.GetComponent<Rigidbody>().AddTorque(Vector3.right * 5f * rndx, ForceMode.Impulse);
        packageSprite.GetComponent<Rigidbody>().AddTorque(Vector3.up * 5f * rndy, ForceMode.Impulse);

        gameManager.soundManager.TicTacSound.Play();
        tntSprite.gameObject.SetActive(true);
        timeSprite.gameObject.SetActive(true);


        isUnpacked = true;
        isRunning = true;
    }

    public override void TryDefuse(string toolName)
    {
        if(defusersList.Contains(toolName))
        {
            tntSprite.SetActive(false);
            timeSprite.SetActive(false);
            smashedSprite.SetActive(true);

            gameManager.soundManager.TicTacSound.Stop();
            gameManager.soundManager.HammerSound.Play();
            isDefused = true;
            isRunning = false;
        }
        if(exploderList.Contains(toolName))
        {
            TriggerExplosion();
            gameManager.soundManager.TicTacSound.Stop();
        }
    }


    public void MoveTimer()
    {
        timeSprite.transform.DOLocalRotate(
            new Vector3(0, 0, 15000),
            60,
            RotateMode.WorldAxisAdd
        )
        .SetEase(Ease.Linear);
    }


}
