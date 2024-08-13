using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Runtime.CompilerServices;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TMP_Text textIndicator;
    public GameObject btnConect;

    private void Start() {
        btnConect.SetActive(false);
    }
    public void ConnectPhoton()
    {
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }

    }
    public void CreatePlayer(string namePlayer)
    {
        PhotonNetwork.NickName = namePlayer;
    }
    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("Conectados a photon");
        textIndicator.text = "Conectados correctamente";
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        textIndicator.text = "Bienvenido" + PhotonNetwork.NickName;
        btnConect.SetActive(true);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }
    public void CreateRoom()
    {
        string user = PhotonNetwork.NickName;
        string nameRoom = "Sala1";
        RoomOptions optionRoom = new RoomOptions();
        optionRoom.IsVisible = true;
        optionRoom.MaxPlayers = 4;
        optionRoom.PublishUserId = true;

        PhotonNetwork.JoinOrCreateRoom(nameRoom,optionRoom,TypedLobby.Default);

    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Estamos Melos sisas sisas"+PhotonNetwork.CurrentRoom.Name + "Bienvenido"+PhotonNetwork.NickName);
    }

}
