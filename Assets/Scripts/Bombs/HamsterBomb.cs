using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HamsterBomb : Bombs
{
    [SerializeField] private int state = 0;

    [Header("References")]
    public GameObject boxSprite;
    public GameObject fatSprite;
    public GameObject slimSprite;
    public GameObject mugSprite;
    public GameObject armSprite;
    public GameObject caseSprite;

    public int maxForce;
    public int minForce;

    public GameObject handReleased;
    public GameObject handClosed;

    public GameObject handPinchSprite;
    public GameObject handFatSprite;
    public GameObject handThrowUpSprite;
    public GameObject handSlimSprite;
    public GameObject throwUpParticles;

    public void Start()
    {
        handReleased = gameManager.handCursor.releasedHand;
        handClosed = gameManager.handCursor.closedHand;

        handPinchSprite = GameObject.Find("HandPinch");
        handFatSprite = GameObject.Find("HandFat");
        handThrowUpSprite = GameObject.Find("ThrowUp");
        handSlimSprite = GameObject.Find("HandSlim");
        throwUpParticles = GameObject.Find("ThrowUpParticles");

    }

    public override void TryDefuse(string toolName)
    {
        if (defusersList.Contains(toolName))
        {
            isDefused = true;

            fatSprite.SetActive(false);
            slimSprite.SetActive(true);
        }
        else if (exploderList.Contains(toolName))
        {
            TriggerExplosion();
        }
    }

    public override void Unpack()
    {
        if (state == 0) // unbox
        {
            boxSprite.SetActive(false);
            fatSprite.SetActive(true);
            mugSprite.SetActive(true);
            armSprite.SetActive(true);
            caseSprite.SetActive(true);

            handReleased.GetComponent<Image>().enabled = false;
            handPinchSprite.GetComponent<Image>().enabled = true;
            state++;
        }
        else if (state == 1) // mug
        {
            //mugSprite.SetActive(false);
            int rndx = Random.Range(minForce, maxForce);
            int rndy = Random.Range(minForce, maxForce);
            mugSprite.GetComponent<Rigidbody>().useGravity = true;
            mugSprite.GetComponent<Rigidbody>().AddForce(Vector3.up * 2f * rndx, ForceMode.Impulse);
            mugSprite.GetComponent<Rigidbody>().AddForce(Vector3.right * 2f * rndy, ForceMode.Impulse);
            mugSprite.GetComponent<Rigidbody>().AddTorque(Vector3.right * 5f * rndx, ForceMode.Impulse);
            mugSprite.GetComponent<Rigidbody>().AddTorque(Vector3.up * 5f * rndy, ForceMode.Impulse);

            handPinchSprite.GetComponent<Image>().enabled = false;
            handReleased.GetComponent<Image>().enabled = true;

            state++;
        }
        else if (state == 2) // hand fat
        {
            fatSprite.SetActive(false);
            armSprite.SetActive(false);

            handFatSprite.GetComponent<Image>().enabled = true;
            handSlimSprite.GetComponent<Image>().enabled = false;

            handPinchSprite.GetComponent<Image>().enabled = false;
            handReleased.GetComponent<Image>().enabled = false;
            state++;
        }
        else if(state == 3) // hand slim
        {
            handFatSprite.GetComponent<Image>().enabled = false;
            handClosed.GetComponent<Image>().enabled = true;
            handThrowUpSprite.GetComponent<Image>().enabled = true;
            throwUpParticles.GetComponent<ParticleSystem>().Play();

            state++;


            DOVirtual.DelayedCall(2, () =>
            {
                handClosed.GetComponent<Image>().enabled = false;
                handThrowUpSprite.GetComponent<Image>().enabled = false;
                handSlimSprite.GetComponent<Image>().enabled = true;
                throwUpParticles.GetComponent<ParticleSystem>().Stop();

            });

        }
        else if(state == 4 && handClosed.GetComponent<Image>().enabled == false) // repose le slim
        {
            slimSprite.SetActive(true);

            handSlimSprite.GetComponent<Image>().enabled = false;
            handReleased.GetComponent<Image>().enabled = true;

            isUnpacked = true;
            isDefused = true;
        }


    }
}
