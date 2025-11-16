using UnityEngine;

public class Glass : Tools
{
    public GameObject verticalGlass;
    public GameObject leanGlass;
    public AudioSource waterSound;

    public override void ActivateTool()
    {
        if (gameManager.currentTool != null)
            if (gameManager.currentTool.toolName == toolName)
            {
                //verticalGlass.SetActive(false);
                //leanGlass.SetActive(true);

                verticalGlass.transform.Rotate(0, 0, 45);
            }
        
    }

    public override void DeactivateTool()
    {
        if (gameManager.currentTool != null)
            if (gameManager.currentTool.toolName == toolName)
            {
                //verticalGlass.SetActive(true);
                //leanGlass.SetActive(false);

                verticalGlass.transform.rotation = Quaternion.identity;
            }
    }

}
