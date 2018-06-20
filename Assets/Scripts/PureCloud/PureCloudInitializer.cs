using Assets.Scripts.PureCloud;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PureCloudInitializer : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(PureCloud.Instance.LoadUsers());
        StartCoroutine(PureCloud.Instance.LoadPresences());

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
