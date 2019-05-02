using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour {

    public GameManager manager;
    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = "" + ((int)(manager.MatchTime / 60)).ToString("D2") + ":" + ((int)(manager.MatchTime % 60)).ToString("D2");

    }
}
