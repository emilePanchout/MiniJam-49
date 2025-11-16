using UnityEngine;
using DG.Tweening;

public class Trader : Bombs
{
    [SerializeField] private float elapsedTime = 0f;

    public int maxForce;
    public int minForce;
    public bool startMask = false;
    public float maxDuration;
    public float minDuration;

    public float maskDuration;
    [SerializeField] private float maskDistance;

    [Header("References")]
    public GameObject mask;
    public GameObject baseCurveSprite;
    public GameObject newCurveSprite;
    public GameObject curveSprite;

    public GameObject handReleased;
    public GameObject handClosed;

    public void Start()
    {
        elapsedTime = 0f;
        maskDuration = Random.Range(minDuration, maxDuration);

    }

    public void Update()
    {
        base.Update();
        if(isDraggable && !startMask)
        {
            startMask = true;
            elapsedTime = 0f;
            MoveMask();
        }

        if (startMask && elapsedTime < maskDuration)
        {
            elapsedTime += Time.deltaTime;
        }
        else if (elapsedTime > maskDuration && startMask && !isDefused)
        {
            startMask = false;
            TriggerExplosion(gameManager.soundManager.CoinSound, 0.5f);
        }
    }

    public override void TryDefuse(string toolName)
    {
        if(exploderList.Contains(toolName))
        {
            TriggerExplosion(gameManager.soundManager.CoinSound, 0.5f);
        }
    }

    public override void Unpack()
    {
        //ReturnCurve();

        int rndx = Random.Range(minForce, maxForce);
        int rndy = Random.Range(minForce, maxForce);
        baseCurveSprite.GetComponent<Rigidbody>().useGravity = true;
        baseCurveSprite.GetComponent<Rigidbody>().AddForce(Vector3.up * 2f * rndx, ForceMode.Impulse);
        baseCurveSprite.GetComponent<Rigidbody>().AddForce(Vector3.right * 2f * rndy, ForceMode.Impulse);
        baseCurveSprite.GetComponent<Rigidbody>().AddTorque(Vector3.right * 5f * rndx, ForceMode.Impulse);
        baseCurveSprite.GetComponent<Rigidbody>().AddTorque(Vector3.up * 5f * rndy, ForceMode.Impulse);

        baseCurveSprite.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;

        isUnpacked = true;
        isDefused = true;
    }


    public void MoveMask()
    {
        mask.transform.DOLocalMoveX(maskDistance, maskDuration);
    }

    public void ReturnCurve()
    {
        curveSprite.transform.DOLocalRotate(new Vector3(180,0,0), 3, RotateMode.WorldAxisAdd);
    }

}
