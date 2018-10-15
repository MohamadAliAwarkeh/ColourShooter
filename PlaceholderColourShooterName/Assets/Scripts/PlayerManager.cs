using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerSetup))]
[RequireComponent(typeof(PlayerShoot))]
public class PlayerManager : NetworkBehaviour
{

    PlayerHealth pHealth;
    PlayerMotor pMotor;
    PlayerSetup pSetup;
    PlayerShoot pShoot;

    Vector3 originalPosition;
    NetworkStartPosition[] spawnPoints;

    void OnDestroy()
    {
        GameManager.allPlayers.Remove(this);
    }

	void Start ()
    {
        pHealth = GetComponent<PlayerHealth>();
        pMotor = GetComponent<PlayerMotor>();
        pSetup = GetComponent<PlayerSetup>();
        pShoot = GetComponent<PlayerShoot>();

        GameManager gm = GameManager.Instance;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        spawnPoints = GameObject.FindObjectsOfType<NetworkStartPosition>();

        originalPosition = transform.position;
    }

    Vector3 GetInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        return new Vector3(h, 0f, v);
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Vector3 inputDir = GetInput();
        pMotor.MovePlayer(inputDir);
    }

    void Update()
    {
        if (!isLocalPlayer || pHealth.isDead)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            pShoot.Shoot();
        }

        Vector3 inputDir = GetInput();
        if (inputDir.sqrMagnitude > 0.25f)
        {
            pMotor.RotateChassis(inputDir);
        }

        Vector3 turretDir = Utility.GetWorldPointFromScreenPoint(Input.mousePosition, pMotor.playerTurret.position.y) - pMotor.playerTurret.position;
        pMotor.RotateTurret(turretDir);
    }

    public void EnableControls()
    {
        pMotor.Enable();
        pShoot.Enable();
    }

    public void DisableControls()
    {
        pMotor.Disable();
        pShoot.Disable();
    }

    void Respawn()
    {
        StartCoroutine("RespawnRoutine");
    }

    IEnumerator RespawnRoutine ()
    {
        SpawnPoint oldSpawn = GetNearestSpawnPoint();

        transform.position = GetRandomSpawnPosition();

        if (oldSpawn != null)
        {
            oldSpawn.isOccupied = false;
        }

        pMotor.myRB.velocity = Vector3.zero;
        yield return new WaitForSeconds(2f);
        pHealth.Reset();

        EnableControls();
    }

    SpawnPoint GetNearestSpawnPoint()
    {
        Collider[] triggerCollider = Physics.OverlapSphere(transform.position, 3f, Physics.AllLayers, QueryTriggerInteraction.Collide);
        foreach (Collider c in triggerCollider)
        {
            SpawnPoint spawnPoint = c.GetComponent<SpawnPoint>();
            if (spawnPoint != null)
            {
                return spawnPoint;
            }
        }
        return null;
    }

    Vector3 GetRandomSpawnPosition()
    {
        if (spawnPoints != null)
        {
            if (spawnPoints.Length > 0)
            {
                bool foundSpawner = false;
                Vector3 newStartPosition = new Vector3();
                float timeOut = Time.time + 2f;

                while (!foundSpawner)
                {
                    NetworkStartPosition startPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    SpawnPoint spawnPoint = startPoint.GetComponent<SpawnPoint>();

                    if (spawnPoint.isOccupied == false)
                    {
                        foundSpawner = true;
                        newStartPosition = startPoint.transform.position;
                    }

                    if (Time.time > timeOut)
                    {
                        foundSpawner = true;
                        newStartPosition = originalPosition;
                    }
                }

                return newStartPosition;
            }
        }
        return originalPosition;
    }




}
