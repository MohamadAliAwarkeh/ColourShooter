using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float bulletSpeed;

	void Start () {
		
	}
	
	void Update () {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
	}
}
