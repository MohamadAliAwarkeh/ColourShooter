using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lPlayer = lobbyPlayer.GetComponent<LobbyPlayer>();

        PlayerSetup pSetup = gamePlayer.GetComponent<PlayerSetup>();

        pSetup.name = lPlayer.playerName;
        pSetup.playerColour = lPlayer.playerColor;

        PlayerManager pManager = gamePlayer.GetComponent<PlayerManager>();
        if (pManager != null)
        {
            GameManager.allPlayers.Add(pManager);
        }
    }

}
