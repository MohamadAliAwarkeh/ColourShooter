using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : NetworkBehaviour
{

    public Transform playerChassis;
    public Transform playerTurret;
    public float moveSpeed;
    public float chassisRotSpeed;
    public float turretRotSpeed;

    public Rigidbody myRB;

    private bool canMove = false;

    void Start ()
    {
        myRB = GetComponent<Rigidbody>();
	}

    public void Enable()
    {
        canMove = true;
    }

    public void Disable()
    {
        canMove = false;
        myRB.velocity = Vector3.zero;
    }

    public void MovePlayer (Vector3 dir)
    {
        if (canMove)
        {
            Vector3 moveDir = dir * moveSpeed * Time.deltaTime;
            myRB.velocity = moveDir;
        }
	}


    public void FaceDirection (Transform xform, Vector3 dir, float rotSpeed)
    {
        if (dir != Vector3.zero && xform != null)
        {
            Quaternion desiredRot = Quaternion.LookRotation(dir);
            xform.rotation = Quaternion.Slerp(xform.rotation, desiredRot, rotSpeed * Time.deltaTime);
        }
    }

    public void RotateChassis(Vector3 dir)
    {
        if (canMove)
        {
            FaceDirection(playerChassis, dir, chassisRotSpeed);
        }
    }

    public void RotateTurret(Vector3 dir)
    {
        if (canMove)
        {
        
            FaceDirection(playerTurret, dir, turretRotSpeed);
        }
    }
}
