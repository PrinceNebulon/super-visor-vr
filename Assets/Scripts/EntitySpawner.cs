using Assets.Scripts.PureCloud;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EntitySpawner : MonoBehaviour {
    
    public Transform boxPrefab;
    public Transform spawnPoint;
    public int spawnMultiplier = 1;
    public int spawnDispersionFactor = 30;

    private PureCloud pureCloud;
    private System.Random random = new System.Random();

    // Use this for initialization
    void Start () {
        pureCloud = PureCloud.Instance;

        StartCoroutine(pureCloud.GetUsers());
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log("Update method");
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            for (var i = 0; i < spawnMultiplier; i++)
            {
                SpawnUser(GetRandomUserWithPicture());
            }
        }
    }

    private void SpawnUser(int i)
    {
        Transform t = Instantiate(boxPrefab);
        //Debug.Log("[OLD] x:" + spawnPoint.position.x + "y:" + spawnPoint.position.y + "z:" + spawnPoint.position.z);
        t.position = new Vector3(
            spawnPoint.position.x + (random.Next(-100 * spawnDispersionFactor, 100 * spawnDispersionFactor) / (float)100),
            spawnPoint.position.y,
            spawnPoint.position.z + (random.Next(-100 * spawnDispersionFactor, 100 * spawnDispersionFactor) / (float)100.0));
        //Debug.Log("[NEW] x:" + t.position.x + "y:" + t.position.y + "z:" + t.position.z);
        t.name = PureCloud.Instance.Users[i].Name;
        Debug.Log(PureCloud.Instance.Users[i].Presence.Id);
        StartCoroutine(applyFaceTexture(t, i));
    }

    private int GetRandomUserWithPicture()
    {
        var i = -1;
        while (i < 0)
        {
            i = random.Next(0, PureCloud.Instance.Users.Count - 1);
            if (PureCloud.Instance.Users[i].Images == null || PureCloud.Instance.Users[i].Images.Count == 0)
            {
                i = -1;
            }
        }
        return i;
    }

    IEnumerator applyFaceTexture(Transform t, int i)
    {   
        Texture2D tex = new Texture2D(96, 96, TextureFormat.DXT1, false);
        using (WWW www = new WWW(PureCloud.Instance.Users[i].Images[0].ImageUri))
        {
            yield return www;
            www.LoadImageIntoTexture(tex);
            t.Find("Face").GetComponent<Renderer>().material.mainTexture = tex;
        }
    }
}
