using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public float moveSpeed;

    private Rigidbody myRB;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Camera mainCam;

    public GunController theGun;
    public bool useController;

	void Start ()
    {
        myRB = GetComponent<Rigidbody>();
        mainCam = FindObjectOfType<Camera>();
	}
	
	void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput * moveSpeed;

        if (!useController)
        {
            Ray cameraRay = mainCam.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }

            if (Input.GetMouseButtonDown(0))
            {
                theGun.isFiring = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                theGun.isFiring = false;
            }
        }

        if (useController)
        {
            Vector3 playerDirection = Vector3.right * Input.GetAxisRaw("RHorizontal") + Vector3.forward * -Input.GetAxis("RVertical");
            if (playerDirection.sqrMagnitude > 0.0f)
            {
                transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
            }

            if (Input.GetKeyDown(KeyCode.Joystick1Button5))
            {
                theGun.isFiring = true;
            }

            if (Input.GetKeyUp(KeyCode.Joystick1Button5))
            {
                theGun.isFiring = false;
            }
        }
	}

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        myRB.velocity = moveVelocity;
    }
}
