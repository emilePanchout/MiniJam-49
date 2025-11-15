using UnityEngine;

public class Pince : Tools
{
    public GameObject closedPince;
    public GameObject openPince;
    public AudioSource squeak;

    public override void TakeTool()
    {
        if (gameManager.currentTool != null)
            if (gameManager.currentTool.toolName == toolName)
            {
                closedPince.SetActive(false);
                openPince.SetActive(true);
            }
    }

    public override void DropTool()
    {
        if (gameManager.currentTool != null)
            if (gameManager.currentTool.toolName == toolName)
            {
                closedPince.SetActive(true);
                openPince.SetActive(false);
            }
    }

    public override void ActivateTool()
    {
        if (gameManager.currentTool != null)
            if (gameManager.currentTool.toolName == toolName)
            {
                closedPince.SetActive(true);
                openPince.SetActive(false);

                squeak.Play();
            }
    }

    public override void DeactivateTool()
    {
        if (gameManager.currentTool != null)
            if (gameManager.currentTool.toolName == toolName)
            {
                closedPince.SetActive(false);
                openPince.SetActive(true);
            }
    }
}
