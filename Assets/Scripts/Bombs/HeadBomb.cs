using UnityEngine;

public class HeadBomb : Bombs
{
    [SerializeField] private int state = 0;

    public int maxBombTimer = 20;
    [SerializeField] private float elapsedTime = 0f;
    [SerializeField] private bool isRunning = true;

    public int maxForce;
    public int minForce;

    [Header("References")]
    public GameObject topSprite;
    public GameObject backSprite;
    public GameObject frontSprite;
    public GameObject bombOnSprite;
    public GameObject bombOffSprite;
    public GameObject shadowSprite;



    void Start()
    {
        elapsedTime = 0f;
        isRunning = true;
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

    public override void TryDefuse(string toolName)
    {
        if (defusersList.Contains(toolName))
        {
            isDefused = true;
            isRunning = false;

            bombOnSprite.SetActive(false);
            bombOffSprite.SetActive(true);
        }
        else if (exploderList.Contains(toolName))
        {
            TriggerExplosion();
        }
    }

    public override void Unpack()
    {
        if(state == 0)
        {
            //topSprite.SetActive(false);

            int rndx = Random.Range(minForce, maxForce);
            int rndy = Random.Range(minForce, maxForce);
            topSprite.GetComponent<Rigidbody>().useGravity = true;
            topSprite.GetComponent<Rigidbody>().AddForce(Vector3.up * 2f * rndx, ForceMode.Impulse);
            topSprite.GetComponent<Rigidbody>().AddForce(Vector3.right * 2f * rndy, ForceMode.Impulse);
            topSprite.GetComponent<Rigidbody>().AddTorque(Vector3.right * 5f * rndx, ForceMode.Impulse);
            topSprite.GetComponent<Rigidbody>().AddTorque(Vector3.up * 5f * rndy, ForceMode.Impulse);

            state++;
        }
        else if(state == 1)
        {
            backSprite.SetActive(false);
            frontSprite.SetActive(false);
            isUnpacked = true;
        }

    }
}
