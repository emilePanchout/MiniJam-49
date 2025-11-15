using UnityEngine;

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
        if (state == 0)
        {
            boxSprite.SetActive(false);
            state++;
        }
        else if (state == 1)
        {
            mugSprite.SetActive(false);
            isUnpacked = true;
        }

    }
}
