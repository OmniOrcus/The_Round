using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIRole : MonoBehaviour {

    public AIAction[] actions;
    protected AIAction currentAction;
    public RoundAI Body { get; private set; }

    void Start()
    {
        Debug.Log(gameObject.name + " AI Role Setup.");
        Body = gameObject.GetComponent<RoundAI>();
        foreach (AIAction action in actions) {
            action.RegisterMind(this);
            action.enabled = false;
        }
        SelectAction();
    }

    public abstract void SelectAction();

}
