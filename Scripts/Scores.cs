using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scores : MonoBehaviour, IObservable {

    public int[] scores;
    public SoundSystem sounds;

    public void ScorePoints(uint team)
    {
        sounds.PlaySound(0, true);
        scores[team] += 1;
        InformObservers();
    }

    //Observer Pattern
    List<IObserver> observers = new List<IObserver>();
    public void RegisterObserver(IObserver observer)
    {
        Debug.Log("Registeing Observer");
        observers.Add(observer);
    }

    public void InformObservers()
    {
        foreach (IObserver observer in observers)
        {
            observer.Observe();
        }
    }
}
