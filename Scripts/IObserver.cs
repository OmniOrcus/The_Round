using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver {
    void Observe();
	
}

public interface IObservable
{
    void RegisterObserver(IObserver observer);
    void InformObservers();

}
