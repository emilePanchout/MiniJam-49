using UnityEngine;

public class MagicBomb : Bombs
{
    [Header("References")]
    public GameObject topSprite;
    public GameObject chesseSprite;
    public GameObject botSprite;
    public ParticleSystem fuseParticules;

    public int maxForce;
    public int minForce;

    public override void Unpack()
    {
        int rndx = Random.Range(minForce, maxForce);
        int rndy = Random.Range(minForce, maxForce);
        topSprite.GetComponent<Rigidbody>().useGravity = true;
        topSprite.GetComponent<Rigidbody>().AddForce(Vector3.up * 2f * rndx, ForceMode.Impulse);
        topSprite.GetComponent<Rigidbody>().AddForce(Vector3.right * 2f * rndy, ForceMode.Impulse);
        topSprite.GetComponent<Rigidbody>().AddTorque(Vector3.right * 5f * rndx, ForceMode.Impulse);
        topSprite.GetComponent<Rigidbody>().AddTorque(Vector3.up * 5f * rndy, ForceMode.Impulse);

        isUnpacked = true;
    }

    public override void TryDefuse(string toolName)
    {

        if (defusersList.Contains(toolName))
        {
            fuseParticules.Stop();
            isDefused = true;
        }
        if (exploderList.Contains(toolName))
        {
            TriggerExplosion();

        }
    }

}
