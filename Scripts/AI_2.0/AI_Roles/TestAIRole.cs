using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAIRole : AIRole {

    public override void SelectAction()
    {
        //clear action
        if (currentAction != null) {
            currentAction.enabled = false;
        }

        //Select new action
        if (Body.HasBall)
        {
            currentAction = actions[1];
        }
        else
        {
            currentAction = actions[0];
        }

        //Activate Action
        currentAction.SetTarget();
        currentAction.enabled = true;
    }
	
}
