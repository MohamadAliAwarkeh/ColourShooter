using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunController : NetworkBehaviour {

    public bool isFiring;
    public BulletController bullet;
    public float bulletSpeed;
    public float timeBetweenShots;
    public Transform fireFrom;

    private float shotCounter;

	void Start () {
		
	}
	
	void Update () {
        if (isFiring)
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = timeBetweenShots;
                BulletController newBullet = Instantiate(bullet, fireFrom.position, fireFrom.rotation) as BulletController;
                newBullet.bulletSpeed = bulletSpeed;
            }
        }
        else
        {
            shotCounter = 0;
        }
	}
}
