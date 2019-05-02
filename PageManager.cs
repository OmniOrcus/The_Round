using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour, IObservable {

    public GameObject[] pages;
    public int Page { get; private set; }

    // Use this for initialization
    void Awake () {
        Page = 0;
	}

    public void TurnPage(int turn) {
        pages[Page].SetActive(false);
        Page += turn;
        if (Page < 0) { Page = pages.Length + Page; }
        else
        {
            Page %= pages.Length;
        }
        Debug.Log("Page turned to " + Page);
        pages[Page].SetActive(true);
        InformObservers();
    }

    private List<IObserver> observers = new List<IObserver>();

    public void InformObservers()
    {
        foreach (IObserver observer in observers) {
            observer.Observe();
        }
    }

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

}
