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
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }

}
