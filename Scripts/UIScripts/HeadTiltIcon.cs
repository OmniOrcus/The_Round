using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTiltIcon : MonoBehaviour, IObserver {

    public RoundPlayer player;
    public float tiltWarning = -90;

    void Start()
    {
        player.RegisterObserver(this);
        Observe();
    }

    public void Observe()
    {
        gameObject.SetActive(player.lookTrack < tiltWarning);
    }

}
