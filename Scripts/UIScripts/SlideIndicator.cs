using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideIndicator : MonoBehaviour, IObserver {

    public RoundPlayer player;
    Image image;

    void Start()
    {
        player.RegisterObserver(this);
        image = GetComponent<Image>();
        Observe();
    }

    public void Observe()
    {
        image.enabled = player.InSlide();
    }

}
