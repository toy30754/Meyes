using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class control : MonoBehaviour {

    public GameObject mgameObject;

    void Start()
    {
        GameObject ngameObject = GameObject.Instantiate(mgameObject);
        ngameObject.transform.SetParent(this.transform, false);
        //ngameObject.GetComponent<ImageTargetBehaviour>().ImageTarget.
    }
}
