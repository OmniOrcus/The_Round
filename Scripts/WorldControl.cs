using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldControl : MonoBehaviour {

    public static WorldControl singleton;
    public static Ball ball;

    public float radius;
    public float gravity = 1;
    public float StartVelocity = 1f;
    public Ball ballFab;
    public bool inPlay = false;

    void Awake() {
        singleton = this;
    }

	void Start () {
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnNewBall();
        }*/
        if (Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene(0);
        }

    }

    public void SpawnNewBall() {
        if (!inPlay)
        {
            Debug.Log("Spawning Ball");
            ball = Instantiate(ballFab, Vector3.zero, Quaternion.identity);
            inPlay = true;
            
            ball.Throw(StartVelocity * (Quaternion.AngleAxis(Random.Range(-180f, 180f), Vector3.forward) * Vector3.up));

            ball.Sound.PlaySound((uint)Ball.SFX.Spawn);

        }
    }

    
}
