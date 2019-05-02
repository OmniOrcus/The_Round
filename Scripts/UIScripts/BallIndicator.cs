using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallIndicator : MonoBehaviour, IObserver{

    public RoundPlayer player;
    Image image;

	// Use this for initialization
	void Start () {
        player.RegisterObserver(this);
        image = GetComponent<Image>();
        Observe();
	}

    public void Observe()
    {
        //Debug.Log("Observing Player");
        image.enabled = player.HasBall;
    }
}
