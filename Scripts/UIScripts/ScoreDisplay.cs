using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour, IObserver {

    public Scores scores;
    public string split;
    Text text;

    // Use this for initialization
    void Start () {
        scores.RegisterObserver(this);
        text = GetComponent<Text>();
        Observe();
	}

    public void Observe()
    {
        text.text = (scores.scores[0].ToString() + split + scores.scores[1].ToString());
    }

}
