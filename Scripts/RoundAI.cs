using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundAI : RoundPlayer
{
    public int currentBehavour = 0;
    public Goal OwnGoal;
    public Goal OppGoal;

    void Start()
    {
        HasBall = false;
        WalkSpeed = SpeedToAngle(WalkSpeed);
        StrafeSpeed = SpeedToAngle(StrafeSpeed);
        flipMotion = flipTime;
    }

    // Update is called once per frame
    void Update () {
        if (slideTrack <= 0)
        {
            if (flipMotion < flipTime) {
                ContinueFlip();
            }
            if (throwTrack <= 0)
            {
                AutoCatch();
            }
        }
        else {
            Slide();
            if (flipMotion < flipTime)
            {
                ContinueFlip();
            }
        }

        Cooldowns();

        Orientate();
        GravityCheck();
    }

    

}
