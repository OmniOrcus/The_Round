using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBallAction : AIAction {

    public RoundPlayer closestPlayer;
    public float InterceptionAimLead = 1.0f;
    public float CatchPointAccuracy;
    public float CatchPointDefenseRange;
    public float ShootAccuracy;
    public float FlipAccuracy;
    public float RushBackRequirement;

    Vector3 interceptTarget = Vector3.zero;



    public override void SetTarget()
    {
        //throw new System.NotImplementedException();
    }

    protected override void Act()
    {
        if (WorldControl.ball != null) //Ball must exist in world
        {
            //Ball heading for own goal
            if (VectorComponent(Ball.Indicator.transform.position - mind.Body.OwnGoal.transform.position, mind.Body.OwnGoal.transform.rotation * Vector3.up) <= 0) {
                Debug.Log("#DEBUG# " + gameObject.name + " needs to intercept: " + VectorComponent(Ball.Indicator.transform.position - mind.Body.OwnGoal.transform.position, mind.Body.OwnGoal.transform.rotation * Vector3.up));
                if ((mind.Body.OwnGoal.transform.position - mind.Body.transform.position).magnitude > RushBackRequirement) {

                    if (AimCheck(mind.Body.OwnGoal.transform.position) < FlipAccuracy)
                    {
                        mind.Body.Flip2();
                    }
                }
                else if (AimCheck(interceptTarget) < ShootAccuracy)
                {
                    Debug.Log("#DEBUG# " + gameObject.name + " makes interception shot.");
                    mind.Body.Shoot();
                }
            }
            //Ball moving in general
            else
            {
                //Standing at intercept point
                if ((Ball.Indicator.transform.position - mind.Body.transform.position).magnitude < CatchPointAccuracy)
                {
                    if (AimCheck(closestPlayer.transform.position) < ShootAccuracy && (closestPlayer.transform.position - mind.Body.transform.position).magnitude < CatchPointDefenseRange)
                    {
                        Debug.Log("#DEBUG# " + gameObject.name + " shoot at player only " + (closestPlayer.transform.position - mind.Body.transform.position).magnitude + " away.");
                        mind.Body.Shoot();
                    }
                }
                else if ((Ball.Indicator.transform.position - mind.Body.transform.position).magnitude * (Ball.Indicator.transform.position - mind.Body.transform.position).magnitude > (WorldControl.singleton.radius * WorldControl.singleton.radius) && AimCheck(Ball.Indicator.transform.position) < FlipAccuracy)
                {

                    mind.Body.Flip2();
                }
            }
        }
    }

    protected override void Aim()
    {
        if (WorldControl.ball != null) {

            if (VectorComponent(Ball.Indicator.transform.position - mind.Body.OwnGoal.transform.position, mind.Body.OwnGoal.transform.rotation * Vector3.up) <= 0)
            {
                if ((mind.Body.OwnGoal.transform.position - mind.Body.transform.position).magnitude > RushBackRequirement)
                {
                    Turn(mind.Body.OwnGoal.transform.position);
                    Look(mind.Body.OwnGoal.transform.position);
                }
                else
                {
                    interceptTarget = WorldControl.ball.transform.position; //WorldControl.ball.transform.position + (WorldControl.ball.velocity * InterceptionAimLead * (WorldControl.ball.transform.position - mind.Body.transform.position).magnitude);
                    Turn(interceptTarget);
                    Look(interceptTarget);
                }
            }
            else {
                if ((Ball.Indicator.transform.position - mind.Body.transform.position).magnitude < CatchPointAccuracy)
                {
                    Turn(closestPlayer.transform.position);
                    Look(closestPlayer.transform.position);
                } 
                else
                {
                    Turn(Ball.Indicator.transform.position);
                    Look(Ball.Indicator.transform.position);
                }
            }
        }
    }

    protected override void Move()
    {
        if (WorldControl.ball != null)
        {
            Walk(Ball.Indicator.transform.position);
            Strafe(Ball.Indicator.transform.position);

        }
        else
        {
            mind.SelectAction();
        }
    }    
}
