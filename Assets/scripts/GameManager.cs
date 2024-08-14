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
    public WindowHandler windowHandler; // Añadir una referencia al WindowHandler
    public int createRoomWindowIndex; // Índice del Canvas CreateRoom en el arreglo windows
    public int startWindowIndex; // Índice del Canvas Start en el arreglo windows
    public GameObject playerNamePrefab; // Prefab del texto que mostrará el nickname del jugador
    public Transform playersContainer; // Contenedor en el que se mostrarán los nombres de los jugadores

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
        // Asegúrate de que estamos conectados al Master Server antes de crear o unirse a una sala
        if (PhotonNetwork.IsConnectedAndReady)
        {
            string nameRoom = "Sala1";
            RoomOptions optionRoom = new RoomOptions();
            optionRoom.IsVisible = true;
            optionRoom.IsOpen = true;
            optionRoom.MaxPlayers = 4; // Máximo 4 jugadores
            optionRoom.PublishUserId = true;

            PhotonNetwork.JoinOrCreateRoom(nameRoom, optionRoom, TypedLobby.Default);
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

        // Activar el Canvas CreateRoom usando WindowHandler y desactivar el Canvas Start
        if (windowHandler != null)
        {
            windowHandler.EnabledWindow(createRoomWindowIndex); // Activa CreateRoom
            windowHandler.EnabledWindow(startWindowIndex); // Desactiva Start
        }

        // Instanciar el prefab de texto con el nombre del jugador
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
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogError("Failed to create the room: " + message);
        textIndicator.text = "Fallo al crear la sala: " + message;
    }
}
