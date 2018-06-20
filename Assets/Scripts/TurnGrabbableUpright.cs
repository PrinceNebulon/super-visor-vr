using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnGrabbableUpright : MonoBehaviour
{
    private GameObject _hand;

	// Use this for initialization
	void Start () {
        _hand = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray ray = new Ray(_hand.transform.position, _hand.transform.forward);
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            var grabbable = hit.collider.GetComponent<OVRGrabbable>();
            if (grabbable != null)
            {
                if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) >= .5)
                {
                    hit.transform.rotation = new Quaternion(0, 0, 0, 0);
                }
            }
        }
	}
}
