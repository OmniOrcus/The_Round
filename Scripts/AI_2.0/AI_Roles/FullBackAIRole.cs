using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullBackAIRole : AIRole {


    public override void SelectAction()
    {

        //clear action
        if (currentAction != null)
        {
            currentAction.enabled = false;
        }

        //Select new action
        if (WorldControl.singleton.inPlay ) {
            if (WorldControl.ball != null )
            {
                if (Ball.Indicator != null)
                {
                    //Chase the Ball
                    currentAction = actions[1];
                }
            }
            else
            {
                if (Body.HasBall)
                {
                    //Throw for Goal
                    currentAction = actions[2];
                } else
                {
                    //Hunt the Ball Carrier
                    currentAction = actions[3];
                }
            }
        }
        else
        {
            //Mark a pre-selected player
            currentAction = actions[0];
        }

        //Activate Action
        currentAction.SetTarget();
        currentAction.enabled = true;

        Debug.Log("#DEBUG# - New Action: " + currentAction.gameObject.name);
    }
}
