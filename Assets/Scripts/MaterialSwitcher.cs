using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour {
    public GameObject target;


	// Use this for initialization
	void Start () {
		
	}
	
	public void SwitchMaterial(Material material)
    {
        target.GetComponent<Renderer>().material = material;
    }
}
