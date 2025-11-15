using UnityEngine;
using System.Collections.Generic;

public class TestBomb : Bombs
{
    public override void TryDefuse(string toolName)
    {
        if(defusersList.Contains(toolName))
        {
            isDefused = true;
        }
        else if(exploderList.Contains(toolName))
        {
            TriggerExplosion();
        }
    }

}
