using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundPlayer : MonoBehaviour, IObservable {

    protected enum SFX {Throw, Shoot, Catch, Flip, Jump, Slide}

    public float WalkSpeed = 1;
    public float StrafeSpeed = 1;

    public float JumpForce = 1;
    public float JumpDamp = 0.5f;

    public float TurnSpeed = 1;
    public float LookSpeed = 1;
    public float LookUpper = 90;
    public float LookLower= -90;
    public GameObject aimHinge;

    public float actionLocationOffset = 1;
    public float catchRadius = 2;
    public float throwStrength = 10;

    protected float jumpTrack = 0;
    public float lookTrack = 0;

    public bool HasBall { get; protected set; }
    public float ThrowCooldown = 1;
    public float throwTrack = 0;

    public Bullet bullet;
    public float ShootCooldown = 1;
    protected float shootTrack = 0;
    public float capLength = 1;
    public float capRadius = 0.5f;

    public float FlipSpeed = 1;
    protected float flipTime;
    protected float flipMotion;
    protected Vector3 flipOrigin;
    protected Vector3 flipDestination;
    protected Quaternion rotStore;
    protected float arcAngle;
    public float FlipCooldown = 1;
    protected float flipTrack = 0;

    public float slideTime = 1;
    protected float slideTrack = 0;
    protected float WalkImpulse;
    protected float StrafeImpulse;

    protected SoundSystem sound;



    // Use this for initialization
    void Awake() {
        sound = GetComponent<SoundSystem>();
    }

    void Start () {
        HasBall = false;
        WalkSpeed = SpeedToAngle(WalkSpeed);
        StrafeSpeed = SpeedToAngle(StrafeSpeed);
        flipTime = (WorldControl.singleton.radius + WorldControl.singleton.radius) / FlipSpeed;
        flipMotion = flipTime;
	}
	
	// Update is called once per frame
	void Update () {

        //Slide handling
        if (slideTrack <= 0)
        {

            //Turn Control
            if (Input.GetAxis("Mouse X") != 0)
            {
                Turn(Input.GetAxis("Mouse X"));
            }

            //Look Control
            if (Input.GetAxis("Mouse Y") != 0)
            {
                Look(Input.GetAxis("Mouse Y"));
                InformObservers();
            }

            //Throw Control
            if (Input.GetAxis("Throw") > 0)
            {
                Throw();
            }

            //Recatch Exception
            if (throwTrack <= 0)
            {
                AutoCatch();
            }

            //Shoot controls
            if (Input.GetAxis("Shoot") > 0)
            {
                Shoot();
            }

            //Flip Exclusion
            if (flipMotion >= flipTime)
            {


                //Walk Control
                if (Input.GetAxis("Walk") != 0)
                {
                    Walk(Input.GetAxis("Walk"));
                }

                //Strafe Control
                if (Input.GetAxis("Strafe") != 0)
                {
                    Strafe(Input.GetAxis("Strafe"));
                }

                //Jump Control
                if (Input.GetAxis("Jump") > 0)
                {
                    Jump();
                }

                //Flip Controls
                if (flipTrack <= 0)
                {
                    if (Grounded() && Input.GetAxis("Flip") > 0)
                    {
                        //Debug.Log(gameObject.name + " Fliping");
                        //TEST OPTIONS - REMOVE LATER
                        Flip2();

                        ;
                    }
                }


                //Jump handling
                if (jumpTrack > 0)
                {
                    //Debug.Log(gameObject.name + " Jumped");
                    transform.position += (-transform.position.normalized * Time.deltaTime * jumpTrack);
                    jumpTrack *= JumpDamp;
                    if (jumpTrack < 0.1)
                    {
                        jumpTrack = 0;
                    }
                }



                //Gravity effect
                GravityCheck();
            }
            else
            {
                //Flip Effect Handling
                ContinueFlip();
            }
        }
        else
        {
            Slide();
            if (flipMotion < flipTime)
            {
                ContinueFlip();
            }
        }

        Cooldowns();

	}

    protected void Cooldowns() {
        //Flip Track
        if (flipTrack > 0) {
            flipTrack -= Time.deltaTime;
        }

        //Shoot Track
        if (shootTrack > 0)
        {
            shootTrack -= Time.deltaTime;
        }

        //Throw Track
        if (throwTrack > 0){
            throwTrack -= Time.deltaTime;
        }
    }

    protected void Slide() {
        Walk(WalkImpulse * slideTrack);
        Strafe(StrafeImpulse * slideTrack);
        slideTrack -= Time.deltaTime;
        InformObservers();
    }

    public bool Grounded() {
        return (transform.position.magnitude + (WorldControl.singleton.gravity * Time.deltaTime) >= WorldControl.singleton.radius);
    }

    public void GravityCheck()
    {
        //Debug.Log("Grav check.");
        if (!Grounded())
        {
            //Debug.Log(gameObject.name + " Falling.");
            transform.position = transform.position.normalized * (transform.position.magnitude + (WorldControl.singleton.gravity * Time.deltaTime));
            Orientate();
        }
    }

    public void Catch()
    {
        if (!HasBall && Vector3.Distance(WorldControl.ball.transform.position, transform.position + ((transform.rotation * Vector3.up)) * actionLocationOffset) <= catchRadius) {
            //Debug.Log(gameObject.name + " Caught Ball");
            Ball.RemoveIndicator();
            Destroy(WorldControl.ball.gameObject);
            HasBall = true;
            InformObservers();
            //SFX
            sound.PlaySound((uint)SFX.Catch, true);
        }
    }

    public void Shoot() {
        if (shootTrack <= 0)
        {
            Debug.Log(gameObject.name + " Shoots At " + (aimHinge.transform.rotation * Vector3.forward));
            Bullet shot = Instantiate(bullet, aimHinge.transform.position + (transform.rotation * aimHinge.transform.localRotation * new Vector3(0, 0, 2.0f)), Quaternion.identity);
            shot.Fire((transform.rotation * aimHinge.transform.localRotation * Vector3.forward) +   new Vector3(), this);
            shootTrack = ShootCooldown;
            //SFX
            sound.PlaySound((uint)SFX.Shoot, true);
        }
    }

    public void Throw()
    {
        if (HasBall && throwTrack <= 0)
        {
            Debug.Log(gameObject.name + " Throwing Ball");
            WorldControl.ball = Instantiate(WorldControl.singleton.ballFab, transform.position + ((transform.rotation * Vector3.up)) * actionLocationOffset, Quaternion.identity);
            WorldControl.ball.Throw((transform.rotation * aimHinge.transform.localRotation * Vector3.forward) * throwStrength);
            HasBall = false;
            throwTrack = ThrowCooldown;
            InformObservers();
            //SFX
            sound.PlaySound((uint)SFX.Throw, true);
        }
    }

    //Flip to the opposite side of the arena
    public void Flip() {
        flipOrigin = transform.position;
        flipDestination = -transform.position;
        arcAngle = 180;
        flipMotion = 0;
        Orientate();
        //SFX
        sound.PlaySound((uint)SFX.Flip, true);
    }

    //Flip to where you are looking
    public void Flip2() {

        if (flipTrack <= 0 && Grounded()) {
            flipTrack = FlipCooldown;
            flipOrigin = transform.position;
            rotStore = transform.rotation;
            //Flip Calculation;
            Vector3 flipVector = transform.rotation * aimHinge.transform.localRotation * Vector3.forward;
            arcAngle = 180 - (2 * Vector3.Angle(-transform.position, flipVector));
            float CordLength = 2 * WorldControl.singleton.radius * Mathf.Sin((Mathf.Deg2Rad * arcAngle) / 2);
            flipDestination = (flipVector.normalized * CordLength) + flipOrigin;
            Debug.Log("Flip Calculation ~ Arc: " + arcAngle + "; Cord: " + CordLength + "; Origin: " + flipDestination.ToString() + "; Destination: " + flipDestination.ToString() + "; Vector: " + flipVector.ToString());

            //Back Flip Excertion
            if (lookTrack < -90)
            {
                arcAngle = -arcAngle;
            }

            flipTime = CordLength / FlipSpeed;

            flipMotion = 0;
            Orientate();
            //SFX
            sound.PlaySound((uint)SFX.Flip, true);
        }
    }

    public void ContinueFlip() {
        flipMotion += Time.deltaTime;
        if (flipMotion >= flipTime)
        {
            transform.position = flipDestination;
            transform.rotation = rotStore;
            transform.RotateAround(transform.position, transform.rotation * -Vector3.right, arcAngle);
            Orientate();
        }
        else
        {
            transform.position = Vector3.Lerp(flipOrigin, flipDestination, (flipMotion / flipTime));
        }

    }

    public float SpeedToAngle(float speed)
    {
        return (speed / (2 * Mathf.PI * WorldControl.singleton.radius)) * 360;
    }

    public void Orientate() {
        //Debug.Log(gameObject.name + " Orientate at " + (-transform.position).ToString());
        transform.rotation = Quaternion.LookRotation(transform.rotation * Vector3.forward, -transform.position);
    }

    //Movement Functions
    public void Turn(float turnDelta) {
        //Debug.Log(gameObject.name + " Turn Movement: " + turnDelta);
        transform.RotateAround(transform.position, -transform.position, turnDelta * TurnSpeed);
    }

    public void Look(float lookDelta) {
        //Debug.Log(gameObject.name + " Look Movement: " + lookDelta);

        if (lookTrack + (-(lookDelta * LookSpeed)) < LookUpper && lookTrack + (-(lookDelta * LookSpeed)) > LookLower)
        {
            //Debug.Log(gameObject.name + " Look Movement: " + lookDelta);
            lookTrack += (-(lookDelta * LookSpeed));
            aimHinge.transform.RotateAround(aimHinge.transform.position, aimHinge.transform.rotation * Vector3.right, -(lookDelta * LookSpeed));
        }
    }

    public void Walk(float walkDelta) {
        //Debug.Log(gameObject.name + " Walks: " + walkDelta);
        transform.RotateAround(Vector3.zero, transform.rotation * -Vector3.right, walkDelta * WalkSpeed * Time.deltaTime);
    }

    public void Strafe(float strafeDelta) {
        //Debug.Log(gameObject.name + " Strafe: " + strafeDelta);
        transform.RotateAround(Vector3.zero, transform.rotation * Vector3.forward, strafeDelta * StrafeSpeed * Time.deltaTime);
    }

    public void Jump() {
        if (Grounded())
        {
            //Debug.Log(gameObject.name + " Jumping");
            jumpTrack = JumpForce;
            //SFX
            sound.PlaySound((uint)SFX.Jump);
        }
    }

    public void GetHit(Vector3 impulse) {

        sound.PlaySound((uint)SFX.Slide);
        Vector3 flatImpulse = Vector3.ProjectOnPlane(impulse, transform.position).normalized;

        //Walk Impulse calc
        Vector3 walkImpulse = Vector3.Project(flatImpulse, (transform.rotation * Vector3.forward));
        WalkImpulse = walkImpulse.magnitude;
        if (Vector3.Angle(walkImpulse, transform.rotation * Vector3.forward) != 0)
            WalkImpulse = -WalkImpulse;

        //Strafe Impulse calc
        Vector3 strafeImpulse = Vector3.Project(flatImpulse, (transform.rotation * Vector3.right));
        StrafeImpulse = strafeImpulse.magnitude;
        if (Vector3.Angle(strafeImpulse, transform.rotation * Vector3.right) != 0)
            StrafeImpulse = -StrafeImpulse;

        slideTrack = slideTime;

        if (HasBall) {
            WorldControl.ball = Instantiate(WorldControl.singleton.ballFab, transform.position + ((transform.rotation * Vector3.up)) * actionLocationOffset, Quaternion.identity);
            WorldControl.ball.Throw(-gameObject.transform.position.normalized * WorldControl.singleton.StartVelocity);
            HasBall = false;
        }

        InformObservers();

    }

    protected void AutoCatch()
    { //Auto-Catch
        if (WorldControl.singleton.inPlay && WorldControl.ball != null)
        {
            Catch();
        }
    }

    //Observer Pattern

    List<IObserver> observers = new List<IObserver>();
    public void RegisterObserver(IObserver observer)
    {
        Debug.Log("Registeing Observer");
        observers.Add(observer);
    }

    public void InformObservers()
    {
        foreach (IObserver observer in observers){
            observer.Observe();
        }
    }

    public bool InFlip()
    {
        return (flipMotion < flipTime);
    }

    public bool InSlide()
    {
        return (slideTrack > 0);
    }

}
