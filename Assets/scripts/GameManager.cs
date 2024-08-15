using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TMP_Text textIndicator;
    public GameObject btnConect;
    public GameObject btnJoinRoom;
    public GameObject btnCreateRoom;
    public GameObject[] windows;
    public int createRoomWindowIndex;
    public int startWindowIndex;
    public GameObject playerNamePrefab;
    public Transform playersContainer;

    private int maxRooms = 10;
    private string baseRoomName = "Sala";

    private void Start()
    {
        btnConect.SetActive(false);
        btnJoinRoom.SetActive(false);
        btnCreateRoom.SetActive(false);
    }

    public void ConnectPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Intentando conectar a Photon...");
        }
    }

    public void CreatePlayer(string namePlayer)
    {
        PhotonNetwork.NickName = namePlayer;
        Debug.Log($"Nombre del jugador establecido: {namePlayer}");
    }

    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("Conectado a Photon");
        textIndicator.text = "Conectado correctamente";
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log($"Conectado al Master Server. Region: {PhotonNetwork.CloudRegion}");
        textIndicator.text = "Bienvenido " + PhotonNetwork.NickName;

        btnConect.SetActive(true);
        btnJoinRoom.SetActive(true);
        btnCreateRoom.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.LogError("Desconectado de Photon: " + cause.ToString());
        textIndicator.text = "Desconectado de Photon: " + cause.ToString();

        btnJoinRoom.SetActive(false);
        btnCreateRoom.SetActive(false);
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.LogError("No estás conectado al Master Server. Espera a que la conexión se complete.");
            textIndicator.text = "Conectando... Por favor espera.";
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No se pudo unir a una sala aleatoria. Creando una nueva...");
        CreateRoom();
    }

    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            for (int i = 1; i <= maxRooms; i++)
            {
                string nameRoom = baseRoomName + i;

                RoomOptions optionRoom = new RoomOptions
                {
                    IsVisible = true,
                    IsOpen = true,
                    MaxPlayers = 2,
                    PublishUserId = true
                };

                PhotonNetwork.CreateRoom(nameRoom, optionRoom, TypedLobby.Default);
                Debug.Log($"Creando sala: {nameRoom}");

                EnabledWindow(createRoomWindowIndex);
                return;
            }
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
        Debug.Log($"Unido a la sala: {PhotonNetwork.CurrentRoom.Name}");

        EnabledWindow(createRoomWindowIndex);

        if (playerNamePrefab != null && playersContainer != null)
        {
            GameObject playerNameObject = Instantiate(playerNamePrefab, playersContainer);
            TMP_Text playerNameText = playerNameObject.GetComponent<TMP_Text>();
            playerNameText.text = PhotonNetwork.NickName;
        }

        Debug.Log($"Jugadores en la sala: {PhotonNetwork.CurrentRoom.PlayerCount}");
        foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            Debug.Log($"Jugador en sala: {player.NickName}");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.LogWarning($"No se pudo unir a la sala. Código: {returnCode}, Mensaje: {message}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogError($"Fallo al crear la sala: {message}");
        textIndicator.text = "Fallo al crear la sala: " + message;

        EnabledWindow(startWindowIndex);
    }

    public void EnabledWindow(int idWindow)
    {
        if (idWindow >= 0 && idWindow < windows.Length)
        {
            for (int i = 0; i < windows.Length; i++)
            {
                windows[i].SetActive(i == idWindow);
            }
        }
        else
        {
            Debug.LogError("ID de ventana inválido.");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Nuevo jugador unido a la sala: {newPlayer.NickName}");
        // Aquí puedes actualizar tu UI o lógica del juego
    }

    public void ListRooms()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.GetCustomRoomList(TypedLobby.Default, "");
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Lista de salas actualizada:");
        foreach (var room in roomList)
        {
            Debug.Log($"Sala: {room.Name}, Jugadores: {room.PlayerCount}/{room.MaxPlayers}");
        }
    }
}