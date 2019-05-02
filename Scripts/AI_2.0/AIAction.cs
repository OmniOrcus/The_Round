using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction : MonoBehaviour {

    protected AIRole mind;

    //

    public void RegisterMind(AIRole mind) {
        this.mind = mind;
    }

    void Update() {
        if (!mind.Body.InSlide())
        {
            Aim();
            Act();
            if (!mind.Body.InFlip())
            {
                Move();
            }
        }
    }

    //Abstract functions
    public abstract void SetTarget();
    protected abstract void Aim();
    protected abstract void Act();
    protected abstract void Move();

    //Aim Functions
    protected void Look(Vector3 target) {
        mind.Body.Look(Clamp(VectorComponent((target - mind.Body.aimHinge.transform.position), (mind.Body.aimHinge.transform.rotation * Vector3.up))));
    }

    protected void Turn(Vector3 target) {
        target = target.normalized * WorldControl.singleton.radius;
        mind.Body.Turn(Clamp(VectorComponent((target - mind.Body.transform.position), (mind.Body.transform.rotation * Vector3.right))));
    }

    protected float AimCheck(Vector3 target) {
        return Vector3.Angle((target - mind.Body.aimHinge.transform.position), (mind.Body.aimHinge.transform.rotation * Vector3.forward));
    }

    //Move Functions
    protected void Walk(Vector3 target) {
        target = target.normalized * WorldControl.singleton.radius;
        mind.Body.Walk(Clamp(VectorComponent((target - mind.Body.transform.position), (mind.Body.transform.rotation * Vector3.forward))));
    }

    protected void Strafe(Vector3 target) {
        target = target.normalized * WorldControl.singleton.radius;
        mind.Body.Strafe(Clamp(VectorComponent((target - mind.Body.transform.position), (mind.Body.transform.rotation * Vector3.right))));
    }

    protected float MoveCheck(Vector3 target) {
        return (target - mind.Body.transform.position).magnitude;
    }




    //#Static Functions

    //Clamps value to between clamp(both positive and negative).
    static protected float Clamp(float input, float clamp = 1.0f) {
        if (input > clamp) { return clamp; }
        if (input < -clamp) { return -clamp; }
        return input;
    }

    //Calculates the component of a on b.
    static protected float VectorComponent(Vector3 a, Vector3 b)
    {
        return (Vector3.Dot(b, a) / b.magnitude);
    }

    //Debuging version
    static protected float VectorComponent(Vector3 a, Vector3 b, bool flag)
    {
        Debug.Log("#VComp# " + Vector3.Dot(b, a) + " : " + b.magnitude + " : A = " + a.normalized.ToString() + " : B = " + b.normalized.ToString());
        return (Vector3.Dot(b, a) / b.magnitude);
    }

}
