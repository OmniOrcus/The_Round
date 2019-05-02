using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public enum SFX {Bounce, Hit, Spawn}

    public float radius;
    public Vector3 velocity;
    public GameObject indicatorPrefab;
    public static GameObject Indicator { get; private set; }
    Transform trans;
    public SoundSystem Sound { get; private set; }

	// Use this for initialization
	void Awake () {
        trans = transform;
        Sound = GetComponent<SoundSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        trans.position += velocity * Time.deltaTime;
        if (trans.position.magnitude + radius >= WorldControl.singleton.radius)
        {
            Bounce();
        }
	}

    void Bounce() {
        //Handle overflow;
        Vector3 pos = trans.position;
        pos *= (WorldControl.singleton.radius / (trans.position.magnitude + radius));
        trans.position = pos;

        //Bounce Calc
        float f = velocity.magnitude;
        velocity += ((-trans.position.normalized) * 2 * f);
        velocity = velocity.normalized * f;

        Indicate();

        //SFX
        Sound.PlaySound((uint)SFX.Bounce, true);

    }

    public static void RemoveIndicator() {
        if (Indicator != null)
        {
            Destroy(Indicator);
        }
    }

    public void Indicate()
    {

        RemoveIndicator();

        //Indicate V2

        float distance = 0;
        float lengthA = transform.position.magnitude;
        float lengthB = WorldControl.singleton.radius;
        float angleB = Vector3.Angle(-transform.position, velocity) * Mathf.Deg2Rad;

        Debug.Log("~Indicator Start: " + lengthA + ", " + lengthB + ", " + angleB + " rad");

        if (angleB == 0) { distance = trans.position.magnitude + WorldControl.singleton.radius; }
        else
        {

            float angleA = Mathf.Asin((lengthA * Mathf.Sin(angleB)) / lengthB);

            float angleC = 180 - ((angleA  + angleB) * Mathf.Rad2Deg);

            distance = (lengthA / Mathf.Sin(angleA)) * Mathf.Sin(angleC * Mathf.Deg2Rad);

            Debug.Log("~Indicator Mid: " + angleA + " rad, " + angleC + " deg");

        }

        Debug.Log("~Indicator end " + distance + ", " + (transform.position + (velocity.normalized * distance)).ToString());

        //Indicator placement
        Indicator = Instantiate(indicatorPrefab, (transform.position + (velocity.normalized * distance)).normalized * WorldControl.singleton.radius, Quaternion.identity);
        //indicator.transform.RotateAround

    }

    public void Throw(Vector3 throwVector) {
        velocity = throwVector;
        if (transform.position == Vector3.zero)
        {
            Invoke("Indicate", 0.1f);
        }
        else {
            Indicate();
        }
    }

    void OnTrigger() { }
}
