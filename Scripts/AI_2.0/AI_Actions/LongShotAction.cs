using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongShotAction : AIAction
{


    public RoundPlayer closestPlayer;//In a team version, this would be set in Set Target;
    public float ThrowAccuracy;
    public float ShootAccuracy;
    public float safeShootReq;

    public override void SetTarget()
    {
        //throw new System.NotImplementedException();
    }

    protected override void Act()
    {
        if (mind.Body.HasBall)
        {
            if (AimCheck(mind.Body.OppGoal.transform.position) < ThrowAccuracy && VectorComponent((mind.Body.transform.position - mind.Body.OwnGoal.transform.position),(mind.Body.OwnGoal.transform.rotation * Vector3.up)) > safeShootReq)
            {
                Debug.Log("#DEBUG# Throw Acc is " + AimCheck(mind.Body.OppGoal.transform.position));
                mind.Body.Throw();
            }
        }
        else if (WorldControl.ball != null)
        {
            if (AimCheck(WorldControl.ball.transform.position) < ShootAccuracy && AimCheck(mind.Body.OppGoal.transform.position) < ShootAccuracy)
            {
                //Debug.Log("#SIM# Shoot at " + ((mind.Body.aimHinge.transform.rotation * Vector3.forward).normalized + mind.Body.transform.position.normalized).normalized * WorldControl.singleton.radius);
                mind.Body.Shoot();
            }
        }
    }

    protected override void Aim()
    {
        Turn(mind.Body.OppGoal.transform.position);
        Look(mind.Body.OppGoal.transform.position);
        /*if (mind.Body.HasBall)
        {
            Turn(mind.Body.OppGoal.transform.position);
            Look(mind.Body.OppGoal.transform.position);
        }
        else if (WorldControl.ball != null)
        {
            Turn(WorldControl.ball.transform.position);
            Look(WorldControl.ball.transform.position);
        }*/
    }

    protected override void Move()
    {
        if (mind.Body.HasBall)
        {
            Walk(-closestPlayer.transform.position);
            Strafe(-closestPlayer.transform.position);
            mind.Body.Jump();
        }
        else if (WorldControl.ball != null)
        {
            if (VectorComponent(Ball.Indicator.transform.position, mind.Body.OppGoal.transform.rotation * Vector3.up) <= 0)
            {

                Vector3 moveTarget = -((WorldControl.ball.velocity + mind.Body.OppGoal.transform.position).normalized * WorldControl.singleton.radius);
                Walk(moveTarget);
                Strafe(moveTarget);

            }
            else
            {
                mind.SelectAction();
            }
        }
        else
        {
            mind.SelectAction();
        }


    }
}
