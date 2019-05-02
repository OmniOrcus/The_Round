using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    TeamManager teamManager;
    public float radius;
    public float force;
    public float speed;

    public RoundPlayer Shooter { get; private set; }  
    public Vector3 Velocity { get; private set; }
    Transform trans;

    // Use this for initialization
    void Start () {
        trans = transform;
        teamManager = GameObject.FindGameObjectWithTag("TeamManager").GetComponent<TeamManager>();
    }
	
	// Update is called once per frame
	void Update () {
        trans.position += Velocity * Time.deltaTime;
        if (trans.position.magnitude + radius >= WorldControl.singleton.radius) {
            //Debug.Log("Bullet OofB");
            Destroy(gameObject);
        }
        if (WorldControl.ball != null) {
            if (Vector3.Distance(trans.position, WorldControl.ball.transform.position) <= (radius + WorldControl.ball.radius)) {
                BallHit();
            }
        }
        if (teamManager.PlayerHitCheck(this))
        {
            Destroy(gameObject);
        }
	}

    public void Fire(Vector3 shotVector, RoundPlayer shooter = null) {
        Velocity = shotVector.normalized * speed;
        Shooter = shooter;
    }

    void BallHit() {
        WorldControl.ball.velocity = (WorldControl.ball.velocity + (Velocity * force)).normalized * WorldControl.ball.velocity.magnitude;
        WorldControl.ball.Sound.PlaySound((uint)Ball.SFX.Hit);
        WorldControl.ball.Indicate();
        Destroy(gameObject);
    }

}
