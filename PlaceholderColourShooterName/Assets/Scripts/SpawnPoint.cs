using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool isOccupied = false;

    void OnTriggerEnter(Collider theCol)
    {
        if (theCol.gameObject.tag == "Player")
        {
            isOccupied = true;
        }
    }

    void OnTriggerStay(Collider theCol)
    {
        if (theCol.gameObject.tag == "Player")
        {
            isOccupied = true;
        }
    }

    void OnTriggerExit(Collider theCol)
    {
        if (theCol.gameObject.tag == "Player")
        {
            isOccupied = false;
        }
    }

}
