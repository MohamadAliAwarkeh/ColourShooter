using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

    public Camera cam;

	void Start () {
        if (cam == null)
        {
            cam = Camera.main;
        }
	}
	
	void Update () {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
	}
}
