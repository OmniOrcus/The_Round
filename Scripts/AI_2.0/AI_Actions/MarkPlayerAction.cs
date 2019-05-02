using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkPlayerAction : AIAction {

    public RoundPlayer PlayerToMark;
    public float GoalDefenceDistance;
    public float SelfDefenceDistance;
    public float ShotAccuracy;
    public float MoveAccuracy;
    public float FlipAccuracy;

    private Vector3 moveTarget;

    /*void Start() {
        Debug.Log("#TEST 1# " + VectorComponent(Vector3.up, Vector3.right));
        Debug.Log("#TEST 2# " + VectorComponent(Vector3.up, Vector3.down));
        Debug.Log("#TEST 3# " + VectorComponent(Vector3.down, Vector3.down));
        Debug.Log("#TEST 4# " + VectorComponent(Vector3.down, Vector3.up));
        Debug.Log("#TEST 5# " + VectorComponent(Vector3.up, new Vector3(0.5f,0.5f,0.5f)));
    }//*/
    

    public override void SetTarget()
    {
        moveTarget = (((PlayerToMark.transform.position + mind.Body.OwnGoal.transform.position) / 2).normalized) * WorldControl.singleton.radius;
    }

    protected override void Act()
    {
        if (((PlayerToMark.transform.position - mind.Body.transform.position).magnitude < SelfDefenceDistance) || ((PlayerToMark.transform.position - mind.Body.OwnGoal.transform.position).magnitude < GoalDefenceDistance)) {
            if (AimCheck(PlayerToMark.transform.position) < ShotAccuracy) {
                mind.Body.Shoot();
            }
        }
        if (AimCheck(moveTarget) < FlipAccuracy && !mind.Body.InFlip()){
            mind.Body.Flip2();
        }
    }

    protected override void Aim()
    {
        //Flip Aim
        if ((moveTarget - mind.Body.transform.position).magnitude * (moveTarget - mind.Body.transform.position).magnitude > 2 * (WorldControl.singleton.radius * WorldControl.singleton.radius)) {
            Turn(moveTarget);
            Look(moveTarget);
        }
        else
        {
            Turn(PlayerToMark.transform.position);
            Look(PlayerToMark.transform.position);
        }
    }

    protected override void Move()
    {
        Walk(moveTarget);
        Strafe(moveTarget);
        if (MoveCheck(moveTarget) < MoveAccuracy) {
            mind.SelectAction();
        }
    }



}
