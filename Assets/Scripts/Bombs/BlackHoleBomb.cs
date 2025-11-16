using UnityEngine;
using UnityEngine.UI;

public class BlackHoleBomb : Bombs
{
    [SerializeField] private int state = 0;
    public int minForce;
    public int maxForce;

    [Header("References")]
    public GameObject blackHoleSprite;
    public GameObject botomBackSprite;
    public GameObject botomFrontSprite;
    public GameObject supportSprite;
    public GameObject topBackSprite;
    public GameObject topFrontSprite;
    public GameObject topSprites;

    public GameObject handReleased;
    public GameObject handClosed;

    public void Start()
    {

        handReleased = gameManager.handCursor.releasedHand;
        handClosed = gameManager.handCursor.closedHand;
    }

    public override void TryDefuse(string toolName)
    {
        if(defusersList.Count > 0 && defusersList.Contains(toolName))
        {
            defusersList.Remove(toolName);
            Destroy(GameObject.Find(toolName));
            gameManager.currentTool = null;

            ResetHand();

        }
        if(defusersList.Count == 0)
        {
            isDefused = true;
        }
    }

    public override void Unpack()
    {
        if(state == 0) // full box
        {
            //topBackSprite.SetActive(false);
            //topFrontSprite.SetActive(false);

            int rndx = Random.Range(minForce, maxForce);
            int rndy = Random.Range(minForce, maxForce);
            topSprites.GetComponent<Rigidbody>().useGravity = true;
            topSprites.GetComponent<Rigidbody>().AddForce(Vector3.up * 2f * rndx, ForceMode.Impulse);
            topSprites.GetComponent<Rigidbody>().AddForce(Vector3.right * 2f * rndy, ForceMode.Impulse);
            topSprites.GetComponent<Rigidbody>().AddTorque(Vector3.right * 5f * rndx, ForceMode.Impulse);
            topSprites.GetComponent<Rigidbody>().AddTorque(Vector3.up * 5f * rndy, ForceMode.Impulse);

            isUnpacked = true;
        }
    }

    public void ResetHand()
    {
        handClosed.GetComponent<Image>().enabled = false;
        handReleased.GetComponent<Image>().enabled = true;
    }
}
