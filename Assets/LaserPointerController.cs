using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointerController : MonoBehaviour {
    private GameObject _pointer;

	// Use this for initialization
	void Start () {
        _pointer = gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) >= .5 &&
            OVRInput.Get(OVRInput.NearTouch.SecondaryThumbButtons))
        {
            _pointer.GetComponent<MeshRenderer>().enabled = true;
        } else
        {
            _pointer.GetComponent<MeshRenderer>().enabled = false;
        }

    }
}
