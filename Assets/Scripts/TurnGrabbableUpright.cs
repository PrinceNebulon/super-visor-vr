using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnGrabbableUpright : MonoBehaviour
{
    private GameObject _hand;
    private int _lastTargetId = -1;

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
            if (objectHit.GetInstanceID() == _lastTargetId)
            {
                // Target hasn't changed since it was set
                Debug.Log("Skipping transform for " + objectHit.name);
                return;
            } else
            {
                // Landed on something else, clear last target
                _lastTargetId = -1;
            }

            var grabbable = hit.collider.GetComponent<OVRGrabbable>();
            if (grabbable != null && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) >= .5)
            {
                // Store last target
                _lastTargetId = objectHit.GetInstanceID();

                // Set rotation
                if (objectHit.CompareTag("NPC"))
                {
                    objectHit.rotation = new Quaternion(0, 0, 0, 0);
                    objectHit.LookAt(GameObject.Find("OVRPlayerController").transform);
                    objectHit.rotation = Quaternion.Euler(new Vector3(0, objectHit.rotation.eulerAngles.y - 90, 0));
                } else
                {
                    objectHit.rotation = new Quaternion(0, 0, 0, 0);
                }
            }
        }
	}
}
