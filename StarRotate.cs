using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRotate : MonoBehaviour {

    public float rotateRate;
    public Vector3 rotateAxis;


	// Use this for initialization
	void Start () {
        rotateAxis.Normalize();
	}
	
	// Update is called once per frame
	void Update () {
        foreach (Transform star in gameObject.transform) {
            star.Rotate(rotateAxis, rotateRate * Time.deltaTime);
        }
	}
}
