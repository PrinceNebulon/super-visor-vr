using Assets.Scripts.PureCloud;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PureCloudInitializer : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        PureCloud.Instance.UserCountChanged += Instance_UserCountChanged;
        PureCloud.Instance.PresenceCountChanged += Instance_PresenceCountChanged;

        StartCoroutine(PureCloud.Instance.LoadUsers());
        StartCoroutine(PureCloud.Instance.LoadPresences());

    }

    private void Instance_PresenceCountChanged(int presenceCount)
    {
        GameObject.Find("PresenceCount").GetComponent<TextMesh>().text = presenceCount.ToString();
    }

    private void Instance_UserCountChanged(int userCount)
    {
        GameObject.Find("UserCount").GetComponent<TextMesh>().text = userCount.ToString();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
