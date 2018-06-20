using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCollider : MonoBehaviour {
    private Vector3 originalPosition;
    private Quaternion originalRotation;

	// Use this for initialization
	void Start () {
        originalPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        originalRotation = new Quaternion(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z, gameObject.transform.rotation.w);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ButtonPlate"))
        {
            GameObject.Find("LeftHandAnchor").GetComponent<OVRGrabber>().ForceRelease(gameObject.GetComponent<OVRGrabbable>());
            GameObject.Find("RightHandAnchor").GetComponent<OVRGrabber>().ForceRelease(gameObject.GetComponent<OVRGrabbable>());

            gameObject.transform.position = originalPosition;
            gameObject.transform.rotation = originalRotation;
        }
    }
}
