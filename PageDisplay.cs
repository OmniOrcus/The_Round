using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageDisplay : MonoBehaviour, IObserver {

    public PageManager manager;
    public Color offColor;
    public Color onColor;
    public int page;
    Image image;

    public void Observe()
    {
        if (manager.Page == page) { image.color = onColor; }
        else { image.color = offColor; }
    }

    void Awake() {
        image = GetComponent<Image>();
        manager.RegisterObserver(this);
    }

    void Start () {
        Observe();
	}

}
