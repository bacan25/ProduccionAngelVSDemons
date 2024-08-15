using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TMP_Text textIndicator;
    public GameObject btnConect;
    public GameObject[] windows; // Array to hold the different UI windows
    public int createRoomWindowIndex; // Index for CreateRoom Canvas in the windows array
    public int startWindowIndex; // Index for Start Canvas in the windows array
    public GameObject playerNamePrefab; // Prefab for displaying player's nickname
    public Transform playersContainer; // Container where player names will be displayed

    private void Start() {
        btnConect.SetActive(false);
    }

    public void ConnectPhoton()
    {
        if (!PhotonNetwork.IsConnected)
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
        textIndicator.text = "Bienvenido " + PhotonNetwork.NickName;
        btnConect.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.LogError("Desconectado de Photon: " + cause.ToString());
        textIndicator.text = "Desconectado de Photon: " + cause.ToString();
    }

    public void CreateRoom()
    {
        // Ensure we are connected to the Master Server before creating or joining a room
        if (PhotonNetwork.IsConnectedAndReady)
        {
            string nameRoom = "Sala1";
            RoomOptions optionRoom = new RoomOptions();
            optionRoom.IsVisible = true;
            optionRoom.IsOpen = true;
            optionRoom.MaxPlayers = 4; // Maximum 4 players
            optionRoom.PublishUserId = true;

            PhotonNetwork.JoinOrCreateRoom(nameRoom, optionRoom, TypedLobby.Default);

            // Change the canvas after attempting to create/join the room
            Debug.Log("Attempting to create/join room: " + nameRoom);
            EnabledWindow(createRoomWindowIndex); // Switch to the CreateRoom canvas
        }
        else
        {
            Debug.LogError("No estás conectado al Master Server. Espera a que la conexión se complete.");
            textIndicator.text = "Conectando... Por favor espera.";
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Estamos Melos sisas sisas " + PhotonNetwork.CurrentRoom.Name + " Bienvenido " + PhotonNetwork.NickName);

        // Canvas switch should have already happened, but ensure it here if necessary
        EnabledWindow(createRoomWindowIndex); // Ensure CreateRoom canvas is active

        // Instantiate the text prefab with the player's nickname
        if (playerNamePrefab != null && playersContainer != null)
        {
            GameObject playerNameObject = Instantiate(playerNamePrefab, playersContainer);
            TMP_Text playerNameText = playerNameObject.GetComponent<TMP_Text>();
            playerNameText.text = PhotonNetwork.NickName;
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.LogError("Failed to join the room: " + message);
        textIndicator.text = "Fallo al unirse a la sala: " + message;

        // Optionally, switch back to the start window if room join fails
        EnabledWindow(startWindowIndex);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogError("Failed to create the room: " + message);
        textIndicator.text = "Fallo al crear la sala: " + message;

        // Optionally, switch back to the start window if room creation fails
        EnabledWindow(startWindowIndex);
    }

    public void EnabledWindow(int idWindow)
    {
        if (idWindow >= 0 && idWindow < windows.Length)
        {
            windows[idWindow].SetActive(true);

            for (int i = 0; i < windows.Length; i++)
            {
                if (idWindow != i)
                    windows[i].SetActive(false);
            }
        }
        else
        {
            Debug.LogError("ID de ventana inválido.");
        }
    }
}
