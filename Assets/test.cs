using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class test : MonoBehaviour {

    // Use this for initialization

    public GameObject abc;

	void Start () {
        abc.GetComponent<ImageTargetBehaviour>().name = "hog";

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
