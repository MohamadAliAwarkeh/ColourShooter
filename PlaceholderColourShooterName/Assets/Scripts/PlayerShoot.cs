using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{

    public Rigidbody bulletPrefab;
    public Transform bulletFireFrom;
    public float timeBetweenShots = 0.5f;
    public float reloadTime = 1f;

    private float shotsLeft;
    private bool isReloading;
    private bool canShoot = false;

	void Start ()
    {
        shotsLeft = timeBetweenShots;
        isReloading = false;
	}
	
	public void Enable ()
    {
        canShoot = true;
	}

    public void Disable()
    {
        canShoot = false;
    }

    public void Shoot()
    {
        if (isReloading || bulletPrefab == null || !canShoot)
        {
            return;
        }

        CmdShoot();

        shotsLeft--;
        if (shotsLeft <= 0)
        {
            StartCoroutine("Reload");
        }
    }

    [Command]
    void CmdShoot()
    {
        Bullet bullet = null;
        bullet = bulletPrefab.GetComponent<Bullet>();
        Rigidbody myRB = Instantiate(bulletPrefab, bulletFireFrom.position, bulletFireFrom.rotation) as Rigidbody;

        if (myRB != null)
        {
            myRB.velocity = bullet.bulletSpeed * bulletFireFrom.transform.forward;
            NetworkServer.Spawn(myRB.gameObject);
        }
    }

    IEnumerator Reload()
    {
        shotsLeft = timeBetweenShots;
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }
}
