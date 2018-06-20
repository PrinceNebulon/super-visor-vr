using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRaycast : MonoBehaviour {
    public Camera camera;
    public GameObject rightHand;
    public Material material;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray ray = new Ray(rightHand.transform.position, rightHand.transform.forward);
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            var grabbable = hit.collider.GetComponent<OVRGrabbable>();
            if (grabbable != null)
            {
                Debug.Log("Raycast hit: " + hit.transform.name + " (" + OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) + ")");
                if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) >= .5)
                {
                    Debug.Log("Setting rotation");
                    hit.transform.rotation = new Quaternion(0, 0, 0, 0);
                }
            }
        }
	}
}
