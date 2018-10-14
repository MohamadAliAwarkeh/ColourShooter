using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletController : NetworkBehaviour {

    public float bulletSpeed;

	void Start () {
		
	}
	
	void Update () {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
	}
}
