using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultMessage : MonoBehaviour {

    public Scores scores;
    public string winText;
    public string drawText;
    public string loseText;
    Text text;

    void Awake() {
        text = GetComponent<Text>();
    }

	// Use this for initialization
	void Start () {
        if (scores.scores[0] > scores.scores[1]) { text.text = loseText; }
        if (scores.scores[0] == scores.scores[1]) { text.text = drawText; }
        if (scores.scores[0] < scores.scores[1]) { text.text = winText; }
	}
	
}

