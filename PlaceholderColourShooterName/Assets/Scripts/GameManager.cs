using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Prototype.NetworkLobby;

public class GameManager : NetworkBehaviour
{
    public Text messageText;

    public static List<PlayerManager> allPlayers = new List<PlayerManager>();

    static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    instance = new GameObject().AddComponent<GameManager>();
                }
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Server]
    void Start()
    {
       StartCoroutine("GameLoopRoutine");
    }

    IEnumerator GameLoopRoutine()
    {
        LobbyManager lobbyManager = LobbyManager.s_Singleton;

        if (lobbyManager != null)
        {
            while (allPlayers.Count < lobbyManager._playerNumber)
            {
                yield return null;
            }

            yield return new WaitForSeconds(2f);
            yield return StartCoroutine("StartGame");
            yield return StartCoroutine("PlayGame");
            yield return StartCoroutine("EndGame");
            StartCoroutine("GameLoopRoutine");
        }
        else
        {
            Debug.LogWarning(" ============== GAMEMANAGER WARNING! Launch game from the lobby scene only! ================= ");
        }
    }

    IEnumerator StartGame()
    {
        RpcStartGame();
        yield return new WaitForSeconds(3f);
    }

    [ClientRpc]
    private void RpcStartGame()
    {
        Reset();
        DisablePlayers();
    }

    IEnumerator PlayGame()
    {
        yield return new WaitForSeconds(1f);

        RpcPlayGame();

        yield return null;
    }

    [ClientRpc]
    private void RpcPlayGame()
    {
        EnablePlayers();
    }

    IEnumerator EndGame()
    {
        RpcEndGame();
        yield return null;
    }

    [ClientRpc]
    private void RpcEndGame()
    {
        DisablePlayers();
    }

    void EnablePlayers()
    {
        for (int i = 0; i < allPlayers.Count; i++)
        {
            if (allPlayers[i] != null)
            {
                allPlayers[i].EnableControls();
            } 
        }
    }

    void DisablePlayers()
    {
        for (int i = 0; i < allPlayers.Count; i++)
        {
            if (allPlayers[i] != null)
            {
                allPlayers[i].DisableControls();
            }
        }
    }

    private void Reset()
    {
        for (int i = 0; i < allPlayers.Count; i++)
        {
            PlayerHealth pHealth = allPlayers[i].GetComponent<PlayerHealth>();
            pHealth.Reset();
        }
    }
}
