using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBehaviour : MonoBehaviour {

    protected static float centerOffset = 0.5f;

    public abstract void Run(RoundAI body);
}
