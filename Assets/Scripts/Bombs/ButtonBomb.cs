using UnityEngine;
using DG.Tweening;

public class ButtonBomb : Bombs
{
    public int minForce;
    public int maxForce;

    [Header("References")]
    public GameObject boxSprite;
    public GameObject buttonUpSprite;
    public GameObject buttonDownSprite;
    public GameObject onSprite;
    public GameObject offSprite;

    public override void Unpack()
    {
        boxSprite.SetActive(false);
        int rndx = Random.Range(minForce, maxForce);
        int rndy = Random.Range(minForce, maxForce);
        boxSprite.GetComponent<Rigidbody>().useGravity = true;
        boxSprite.GetComponent<Rigidbody>().AddForce(Vector3.up * 2f * rndx, ForceMode.Impulse);
        boxSprite.GetComponent<Rigidbody>().AddForce(Vector3.right * 2f * rndy, ForceMode.Impulse);
        boxSprite.GetComponent<Rigidbody>().AddTorque(Vector3.right * 5f * rndx, ForceMode.Impulse);
        boxSprite.GetComponent<Rigidbody>().AddTorque(Vector3.up * 5f * rndy, ForceMode.Impulse);

        isUnpacked = true;
        isDraggable = false;
    }

    public override void TryDefuse(string toolName)
    {
        if(toolName == "Screwdriver")
        {
            buttonUpSprite.SetActive(false);
            onSprite.SetActive(true);
        }
        else if(toolName == "Hands")
        {
            buttonDownSprite.SetActive(true);
            buttonUpSprite.SetActive(false);

            gameManager.soundManager.ButtonSound.time = 0.1f;
            gameManager.soundManager.ButtonSound.Play();

            DOVirtual.DelayedCall(1, () =>
            {
                TriggerExplosion();
            });


        }
        else if(toolName == "Pince" && !buttonUpSprite.activeSelf)
        {
            onSprite.SetActive(false);
            offSprite.SetActive(true);

            isDraggable = true;
            isDefused = true;
        }
        else if(exploderList.Contains(toolName))
        {
            TriggerExplosion();
        }
    }


}
