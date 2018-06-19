using Assets.Scripts.PureCloud;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ButtonTrigger : MonoBehaviour {

    [SerializeField]
    private Transform boxPrefab;
    [SerializeField]
    private Transform spawnPoint;

    private PureCloud pureCloud;

	// Use this for initialization
	void Start () {
        //      var accessTokenInfo = Configuration.Default.ApiClient.PostToken("18a4c365-7ea3-4f0g-9fb7-884fb4d2e9c6",
        //"M7FfdYQyL5TA6BdbEZ8M9-Wx4uZai1rNQ7jcuFdcJJo");
        //      Console.WriteLine("Access token=" + accessTokenInfo.AccessToken);

        pureCloud = PureCloud.Instance;

        StartCoroutine(pureCloud.GetUsers());
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log("Update method");
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        Debug.Log(other.tag);
        if (other.CompareTag("Button"))
        {
            Transform t = Instantiate(boxPrefab);
            t.position = spawnPoint.position;

            StartCoroutine(applyFaceTexture(t));
        }
    }

    private int GetRandomUserWithPicture()
    {
        System.Random random = new System.Random();
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

    IEnumerator applyFaceTexture(Transform t)
    {
        var i = GetRandomUserWithPicture();

        Debug.Log("Getting texture from " + PureCloud.Instance.Users[i].Images[0].ImageUri);
        Texture2D tex = new Texture2D(96, 96, TextureFormat.DXT1, false);
        using (WWW www = new WWW(PureCloud.Instance.Users[i].Images[0].ImageUri))
        {
            Debug.Log("Before yield");
            yield return www;
            Debug.Log("Loading into texture");
            www.LoadImageIntoTexture(tex);
            Debug.Log("Applying to transform");
            t.Find("Face").GetComponent<Renderer>().material.mainTexture = tex;
            Debug.Log("Done with tex");
        }
    }

    IEnumerator applyImageTexture(Transform t)
    {
        System.Random random = new System.Random();
        var i = random.Next(0, PureCloud.Instance.Users.Count - 1);

        if (PureCloud.Instance.Users[i].Images == null || PureCloud.Instance.Users[i].Images.Count == 0)
        {
            Debug.LogWarning("No images for " + PureCloud.Instance.Users[i].Name);
            yield return null;
        }

        Debug.Log("Getting texture from " + PureCloud.Instance.Users[i].Images[0].ImageUri);
        Texture2D tex = new Texture2D(96, 96, TextureFormat.DXT1, false);
        using (WWW www = new WWW(PureCloud.Instance.Users[i].Images[0].ImageUri))
        {
            Debug.Log("Before yield");
            yield return www;
            Debug.Log("Loading into texture");
            www.LoadImageIntoTexture(tex);
            Debug.Log("Applying to transform");
            t.GetComponent<Renderer>().material = Resources.Load("PhotoShader", typeof(Material)) as Material;
            t.GetComponent<Renderer>().material.mainTexture = tex;
            Debug.Log("Done with tex");
        }
    }
}
