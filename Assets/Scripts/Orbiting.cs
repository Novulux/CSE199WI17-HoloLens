using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiting : MonoBehaviour {
    private bool orbiting;
    Vector3 relDist;
    Transform target;
    public bool lookAt;
    public bool floating;
	// Use this for initialization
	void Start () {
        target = Camera.main.transform;
        orbiting = false;
        floating = false;
	}

    private void OnEnable()
    {
        //Some objects don't work well with LookAt so we can just set it
        if (lookAt)
        {
            transform.LookAt(target);
        }
    }

    public bool getOrbit()
    {
        return orbiting;
    }
    public void setOrbit(bool val)
    {
        if(val == true)
        {
            orbiting = true;
        }
        else
        {
            orbiting = false;
        }
    }

    //this makes an object orbit with respect to the initial distance, but we could make it adjustable. 
    public void Orbit()
    {
        relDist = transform.position - target.position;
        if (lookAt)
        {
            transform.LookAt(target);
        }
        setOrbit(true);
    }

    // Update is called once per frame
    void LateUpdate () {
        if (orbiting)
        {
            transform.position = (target.position + relDist);
            transform.RotateAround(target.position, Vector3.up, 10 * Time.deltaTime);
            relDist = transform.position - target.position;
           // transform.LookAt(target);
        }
    }
}
