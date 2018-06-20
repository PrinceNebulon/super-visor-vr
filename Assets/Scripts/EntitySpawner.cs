using Assets.Scripts.PureCloud;
using PureCloudPlatform.Client.V2.Model;
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
    
    private System.Random random = new System.Random();

    // Use this for initialization
    void Start () {

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
        // Create new instance of prefab
        Transform prefab = Instantiate(boxPrefab);

        // Determine starting position relative to the spawn point
        prefab.position = new Vector3(
            spawnPoint.position.x + (random.Next(-100 * spawnDispersionFactor, 100 * spawnDispersionFactor) / (float)100),
            spawnPoint.position.y,
            spawnPoint.position.z + (random.Next(-100 * spawnDispersionFactor, 100 * spawnDispersionFactor) / (float)100.0));

        // Face a random direction
        prefab.Rotate(0, random.Next(0, 360), 0);

        // Get PureCloud user
        var user = PureCloud.Instance.Users[i];
        Debug.Log(user.Name + " -> " + PureCloud.Instance.ResolvePresenceId(user.Presence.PresenceDefinition.Id).DefaultLabel);

        // Set prefab name
        prefab.name = user.Name;
        prefab.Find("PlayerName").GetComponent<TextMesh>().text = user.Name;

        //var color = PresenceColors.FromSystemPresence(user.Presence.PresenceDefinition.SystemPresence);
        //npc.Find("Hair").GetComponent<Renderer>().material.SetColor("_Color", color);
        prefab.Find("Hair").GetComponent<Renderer>().material = LoadPresenceMaterial(user.Presence.PresenceDefinition.SystemPresence);
        
        // Download and apply profile picture
        StartCoroutine(applyFaceTexture(prefab, PureCloud.Instance.Users[i].DefaultImage));
    }

    private Material LoadPresenceMaterial(string systemPresence)
    {
        switch (systemPresence.ToUpperInvariant())
        {
            case "AVAILABLE":
                return Resources.Load<Material>("Presence Colors/PresenceAvailable");
            case "AWAY":
            case "MEAL":
            case "TRAINING":
            case "BREAK":
                return Resources.Load<Material>("Presence Colors/PresenceAway"); ;
            case "BUSY":
            case "MEETING":
                return Resources.Load<Material>("Presence Colors/PresenceBusy"); ;
            case "IDLE":
            case "ON_QUEUE":
                return Resources.Load<Material>("Presence Colors/PresenceOnQueue"); ;
            default:
                return Resources.Load<Material>("Presence Colors/PresenceOffline"); ;
        }
    }

    private int GetRandomUserWithPicture()
    {
        var i = -1;
        while (i < 0)
        {
            i = random.Next(0, PureCloud.Instance.Users.Count - 1);
            if (PureCloud.Instance.Users[i].Images == null || PureCloud.Instance.Users[i].Images.Count == 0)
            {
                // Ignore this user, no picture
                i = -1;
            }
        }
        return i;
    }

    IEnumerator applyFaceTexture(Transform t, UserImage image)
    {
        Debug.Log("Resolution: " + image.ResolutionInt);
        Texture2D tex = new Texture2D(image.ResolutionInt, image.ResolutionInt, TextureFormat.DXT1, false);
        using (WWW www = new WWW(image.ImageUri))
        {
            yield return www;
            www.LoadImageIntoTexture(tex);
            t.Find("Face").GetComponent<Renderer>().material.mainTexture = tex;
        }
    }
}
