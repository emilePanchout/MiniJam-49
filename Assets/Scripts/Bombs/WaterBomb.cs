using UnityEngine;
using DG.Tweening;

public class WaterBomb : Bombs
{

    [Header("References")]
    public GameObject boxSprite;
    public GameObject waterSprite;
    public GameObject smashedSprite;

    public int maxForce;
    public int minForce;

    public override void Unpack()
    {
        int rndx = Random.Range(minForce, maxForce);
        int rndy = Random.Range(minForce, maxForce);
        boxSprite.GetComponent<Rigidbody>().useGravity = true;
        boxSprite.GetComponent<Rigidbody>().AddForce(Vector3.up * 2f * rndx, ForceMode.Impulse);
        boxSprite.GetComponent<Rigidbody>().AddForce(Vector3.right * 2f * rndy, ForceMode.Impulse);
        boxSprite.GetComponent<Rigidbody>().AddTorque(Vector3.right * 5f * rndx, ForceMode.Impulse);
        boxSprite.GetComponent<Rigidbody>().AddTorque(Vector3.up * 5f * rndy, ForceMode.Impulse);

        waterSprite.SetActive(true);

        isUnpacked = true;
    }

    public override void TryDefuse(string toolName)
    {

        if (exploderList.Contains(toolName))
        {
            waterSprite.SetActive(false);
            smashedSprite.SetActive(true);

            DOVirtual.DelayedCall(2, () =>
            {
                TriggerExplosion();
            });


        }
    }
}
