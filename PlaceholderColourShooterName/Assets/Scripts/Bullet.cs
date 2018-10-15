using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Bullet : NetworkBehaviour
{
    Rigidbody myRB;
    Collider myCol;
    public int bulletSpeed = 100;
    public float lifeTime = 5f;
    public float damage = 1f;

    public List<string> collisionTags;

	void Start () {
        myRB = GetComponent<Rigidbody>();
        myCol = GetComponent<Collider>();
        StartCoroutine("SelfDestruct");
	}

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(lifeTime);

        Explode();
    }

    void Explode()
    {
        myCol.enabled = false;
        myRB.velocity = Vector3.zero;
        myRB.Sleep();

        if (isServer)
        {
            Destroy(gameObject);
            foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>())
            {
                m.enabled = false;
            }
        }
    }

    void CheckCollision(Collision theCol)
    {
        if (collisionTags.Contains(theCol.collider.tag))
        {
            Explode();
            PlayerHealth playerHealth = theCol.gameObject.GetComponentInParent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Damage(damage);
            }
        }
    }

    void OnCollisionEnter(Collision theCol)
    {
        CheckCollision(theCol);
    }
}
