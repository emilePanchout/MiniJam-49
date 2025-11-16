using UnityEngine;

public class CardboardBomb : Bombs
{
    public GameObject box;
    public GameObject smashed;


    public void Start()
    {
        isDefused = true;
    }
    public override void TryDefuse(string toolName)
    {
        if (defusersList.Contains(toolName))
        {
            box.SetActive(false);
            smashed.SetActive(true);

        }
    }

}
