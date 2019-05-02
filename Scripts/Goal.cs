using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    public uint team;
    public Scores scores;

    // Update is called once per frame
    void Update() {
        if (WorldControl.singleton.inPlay && WorldControl.ball != null)
        {
            float z = WorldControl.ball.transform.position.z;
            if (transform.position.z < 0 && z - WorldControl.ball.radius < transform.position.z) { Score(); }
            if (transform.position.z > 0 && z + WorldControl.ball.radius > transform.position.z) { Score(); }
        }
    }

    void Score(){
        scores.ScorePoints( team);
        Destroy(WorldControl.ball.gameObject);
        Ball.RemoveIndicator();
        WorldControl.singleton.inPlay = false;
    }
}
