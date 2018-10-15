using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerSetup : NetworkBehaviour
{

    [SyncVar(hook = "UpdateColour")] public Color playerColour;
    [SyncVar(hook = "UpdateName")] public string name = "Player";
    public Text playerNameText;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isServer)
        {
            PlayerManager pManager = GetComponent<PlayerManager>();
            if (pManager != null)
            {
                GameManager.allPlayers.Add(pManager);
            }
        }

        UpdateName(name);
        UpdateColour(playerColour);
    }

    private void UpdateColour(Color pColour)
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in meshes)
        {
            r.material.color = pColour;
        }
    }

    private void UpdateName(string pName)
    {
        if (playerNameText != null)
        {
            playerNameText.enabled = true;
            playerNameText.text = name;
        }
    }
}
