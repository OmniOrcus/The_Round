using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IObservable {

    public float MatchTime; //in minutes
    public float BallSpawnDelay = 5f;
    public float SpawnTimer = 0;
    public bool Spawning = false;
    public SoundSystem sound;
    public GameObject endScreen;
    public RoundPlayer[] players;

    void Awake() {
        endScreen.SetActive(false);
    }

	// Use this for initialization
	void Start () {
        MatchTime *= 60;
        StartSpawning();
	}
	
	// Update is called once per frame
	void Update () {
        if(!WorldControl.singleton.inPlay  && !Spawning)
        {
            StartSpawning();
        }

        MatchTime -= Time.deltaTime;
        if (MatchTime <= 0) {
            sound.PlaySound(1, true);
            //game over code
            endScreen.SetActive(true);
            Cursor.visible = true;
            if (WorldControl.ball != null)
            {
                Destroy(WorldControl.ball.gameObject);
            }
            Ball.RemoveIndicator();
            WorldControl.singleton.inPlay = false;
            foreach (RoundPlayer player in players) {
                player.enabled = false;
            }
            enabled = false;
        }
        if (Spawning) {
            SpawnTimer -= Time.deltaTime;
            if (SpawnTimer <= 0) {
                WorldControl.singleton.SpawnNewBall();
                Spawning = false;
            }
        }
	}

    public void StartSpawning() {
        SpawnTimer = BallSpawnDelay;
        Spawning = true;
        InformObservers();
    }

    List<IObserver> observers = new List<IObserver>();

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void InformObservers()
    {
        foreach (IObserver observer in observers) {
            observer.Observe();
        }
    }
}
