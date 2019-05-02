using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDisplay : MonoBehaviour, IObserver {

    public GameManager gameManager;
    Text text;

    public void Observe()
    {
        gameObject.SetActive(true);
    }

    // Use this for initialization
    void Start () {
        text = GetComponent<Text>();
        gameManager.RegisterObserver(this);
	}
	
	// Update is called once per frame
	void Update () {
        text.text = ((int)(gameManager.SpawnTimer + 1)).ToString();
        if (!gameManager.Spawning) {
            gameObject.SetActive(false);
        }
	}


    
}
