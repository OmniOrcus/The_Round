using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockerAI : AIBehaviour {
    //DESCRIPTION: Pos


    public RoundPlayer player;
    public float targetLocationBuffer = 0;
    public float playerLookBuffer = 0;
    public float aimBuffer = 0;
    public float aimTime = 1;
    protected float aimTrack = 0;
    public float shotAccuracy = 5;
    public float throwAccuracy = 5;
    public float throwBackUp = 0.5f;
    public float interceptBuffer = 0.5f;

    void Start() {
        //playerLookBuffer *= playerLookBuffer;
        //targetLocationBuffer *= targetLocationBuffer;
    }

    public override void Run(RoundAI body)
    {
        //Set up oct-splits
        Vector3 LRSplit = Vector3.Cross(body.transform.position, body.transform.rotation * Vector3.forward);
        Vector3 FBSplit = Vector3.Cross(body.transform.position, body.transform.rotation * Vector3.right);

        //rotation calculations



        Vector3 proj = Vector3.Project(player.transform.position, LRSplit);
        float side = (LRSplit.magnitude - (LRSplit - proj).magnitude);

        
        proj = Vector3.Project(player.transform.position, FBSplit);
        float front = (FBSplit.magnitude - (FBSplit - proj).magnitude);

        if (front < 0 && side == 0) { side = 1; }


        //Debug.Log("Rote Check: s=" + side + ", f=" + front + ", s2=" + (side * side) +", r=" + ((side * side) > playerLookBuffer));
        if ((side * side) > playerLookBuffer * playerLookBuffer)
        {
            if (side <= (2 * playerLookBuffer) && side >= -(2 * playerLookBuffer))
            {
                body.Turn(-(side / (2 * playerLookBuffer)));
            }
            else
            {
                body.Turn(Direct(-side));
            }
        }

        //Target calcualtion
        Vector3 target = (((player.transform.position + body.OwnGoal.transform.position) / 2).normalized) * WorldControl.singleton.radius;

        //Debug.Log("Target Check: " + target.ToString());

        //Movement Calculation
        proj = Vector3.Project(target, LRSplit);
        side = (LRSplit.magnitude - (LRSplit - proj).magnitude);

        proj = Vector3.Project(target, FBSplit);
        front = (FBSplit.magnitude - (FBSplit - proj).magnitude);

        //Walking
        if (front * front > targetLocationBuffer * targetLocationBuffer)
        {
            if (front <= (2 * targetLocationBuffer) && front >= -(2 * targetLocationBuffer))
            {
                body.Walk(front/(2 * targetLocationBuffer));
            }
            else
            {
                body.Walk(Direct(front));
            }
        }
        //Strafeing
        if (side * side > targetLocationBuffer * targetLocationBuffer)
        {
            if (side <= (2 * targetLocationBuffer) && side >= -(2 * targetLocationBuffer)) {
                body.Strafe(-(side / (2 * targetLocationBuffer)));
            }
            else {
                body.Strafe(Direct(-side));
            }
        }

        //Aiming calculations - Setup
        //Vector3 AimFlat = body.aimHinge.transform.rotation * Vector3.up;
        //Debug.Log("AimFlat: " + AimFlat.ToString());
        Vector3 look;

        
        if (!body.HasBall)
        {
            if (!ShotBlock(ref body))
            {
                //Intercept check
                if (!Intercept(ref body))
                {
                    //Shot behaviour
                    look = (player.transform.position + ((player.transform.rotation * Vector3.up) * centerOffset)) - body.aimHinge.transform.position;
                    Look(look, ref body, aimBuffer);

                    if (Vector3.Angle(look, body.aimHinge.transform.rotation * Vector3.forward) < shotAccuracy)
                    {
                        body.Shoot();
                    }
                }
            }
        }

        //Throw behavious
        if (body.HasBall) {

            if (Look(body.OppGoal.transform.position, ref body, aimBuffer))
            {
                body.throwTrack = body.ThrowCooldown;
                body.Throw();
            }
            else {
                body.Walk(-throwBackUp);
            }
            
        }

    }

    bool Look(Vector3 target, ref RoundAI body, float buffer = 0.0f) {
        target = target - body.aimHinge.transform.position;
        Vector3 AimFlat = body.aimHinge.transform.rotation * Vector3.up;
        float tilt = SignedMagnitude(Vector3.Project(target, AimFlat), AimFlat);

        if (tilt * tilt > buffer * buffer)
        {
            if (tilt <= (2 * buffer) && tilt >= -(2 * buffer))
            {
                body.Look((tilt / (2 * buffer)));
            }
            else
            {
                body.Look(Direct(tilt));
            }
        }

        return (Vector3.Angle(target, body.aimHinge.transform.rotation * Vector3.forward) < throwAccuracy);

    }

    bool Intercept(ref RoundAI body) {
        if (WorldControl.ball != null && Ball.Indicator != null)
        {
            Debug.Log(body.name + " is Intercepting.");
            if (Vector3.Project(Ball.Indicator.transform.position - body.transform.position, Vector3.forward).magnitude < interceptBuffer)
            {
                
                if (Look(Ball.Indicator.transform.position, ref body, aimBuffer))
                {
                    body.Flip2();
                }
                return true;
            }
        }
        return false;
    }

    bool ShotBlock(ref RoundAI body) {
        if (WorldControl.ball != null && Ball.Indicator != null) {
            if (Ball.Indicator.transform.position.z > body.OwnGoal.transform.position.z)
            {
                Debug.Log(body.name + " is Blocking shot.");
                Vector3 look = Ball.Indicator.transform.position - body.aimHinge.transform.position;
                Look(look, ref body, aimBuffer);
                if (Vector3.Angle(look, body.aimHinge.transform.rotation * Vector3.forward) < shotAccuracy)
                {
                    body.Shoot();
                }
                return true;
            }
        }
        return false;
    }

    int Direct(float num) {
        if (num > 0) return 1;
        if (num < 0) return -1;
        else return 0;
    }

    float SignedMagnitude(Vector3 input, Vector3 comparitor) {
        if (Vector3.Angle(input, comparitor) != 0)
        {
            return -input.magnitude;
        }
        else {
            return input.magnitude;
        }
    }

}
