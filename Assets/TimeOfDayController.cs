using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfDayController : MonoBehaviour {
    public float Speed = 1.0f;
    public float NightSpeed = 4.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //-20 to 190
        //Debug.Log(gameObject.transform.rotation.x + "|" + gameObject.transform.rotation.y + "|" + gameObject.transform.rotation.z);
        Debug.Log(gameObject.transform.rotation.eulerAngles.x + "|" + gameObject.transform.rotation.eulerAngles.y + "|" + gameObject.transform.rotation.eulerAngles.z);
        var speed = 0.1f * Speed;
        if (gameObject.transform.rotation.eulerAngles.x > 200 && gameObject.transform.rotation.eulerAngles.x < 340)
            speed *= NightSpeed;
        gameObject.transform.Rotate(new Vector3(speed, 0, 0));
	}
}
