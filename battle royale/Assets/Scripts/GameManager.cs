using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class GameManager : MonoBehaviourPun
{
    [Header("Players")]
    public string playerPrefabLocation;
    public PlayerController[] players;
    public Transform[] spawnPoints;
    public int alivePlayers;
    
    private int playersInGame;
    
    // instance
    public static GameManager instance;

    void Awake ()
    {
        instance = this;
    }

    [PunRPC]
    void ImInGame ()
    {
        playersInGame++;
        if(PhotonNetwork.IsMasterClient && playersInGame == PhotonNetwork.PlayerList.Length)
        photonView.RPC("SpawnPlayer", RpcTarget.All);
    }

    [PunRPC]
    void SpawnPlayer ()
    {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        // initialize the player for all other players
    }

    // Start is called before the first frame update
    void Start()
    {
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        alivePlayers = players.Length;
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
