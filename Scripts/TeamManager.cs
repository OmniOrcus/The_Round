using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {

    public RoundPlayer[] players;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool PlayerHitCheck(Bullet shot)
    {
        Vector3 flatProject;
        Vector3 backProject;
        foreach (RoundPlayer player in players) {

            if (player != shot.Shooter)
            {

                flatProject = Vector3.ProjectOnPlane(shot.transform.position, -(player.transform.position).normalized);
                backProject = Vector3.Project(shot.transform.position, player.transform.rotation * Vector3.right);

                //Debug.Log(player.name + " hit check: f=" + flatProject.magnitude + ", fReq= " + (player.capRadius + shot.radius) + ", b=" + backProject.magnitude + ", bReq=" + (player.capLength + player.capRadius + shot.radius));

                if (flatProject.magnitude <= (player.capRadius + shot.radius) && backProject.magnitude <= (player.capLength + player.capRadius + shot.radius))
                {
                    Debug.Log(player.name + " is hit: " + Vector3.ProjectOnPlane(shot.Velocity, -player.transform.position).ToString());

                    player.GetHit(shot.Velocity);
                    return true;
                }
            }

        }
        return false;
    }

}
