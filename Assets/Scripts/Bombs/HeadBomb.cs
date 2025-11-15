using UnityEngine;

public class HeadBomb : Bombs
{
    [SerializeField] private int state = 0;

    public int maxBombTimer = 20;
    [SerializeField] private float elapsedTime = 0f;
    [SerializeField] private bool isRunning = true;

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
            topSprite.SetActive(false);
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
