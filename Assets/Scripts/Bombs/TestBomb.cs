using UnityEngine;
using System.Collections.Generic;

public class TestBomb : Bombs
{
    public List<string> defusersList;
    public List<string> exploderList;
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
