using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerSetup : NetworkBehaviour {

    public Color playerColour;
    public string baseName = "Player";
    public int playerNum = 1;
    public Text playerNameText;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (playerNameText != null)
        {
            playerNameText.enabled = false;
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //This sets the local players colour, so you can differeniate who you are from the other player(s)
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in meshes)
        {
            r.material.color = playerColour;
        }

        //This sets your name so everyone knows who you are
        if (playerNameText != null)
        {
            playerNameText.enabled = true;
            playerNameText.text = baseName + playerNum.ToString();
        }
    }
}
