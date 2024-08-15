using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

[RequireComponent(typeof(PhotonView))]
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

        if (photonView == null)
        {
            Debug.LogError("PhotonView no está asignado.");
        }
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

        // Entrar al lobby automáticamente para que el usuario pueda ver la lista de salas si es necesario
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Unido al Lobby");
        textIndicator.text = "Unido al Lobby";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.LogError("Desconectado de Photon: " + cause.ToString());
        textIndicator.text = "Desconectado de Photon: " + cause.ToString();

        btnJoinRoom.SetActive(false);
        btnCreateRoom.SetActive(false);
    }

    // Este método solo debe ser llamado cuando se presiona el botón "Unirse a Sala"
    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.LogError("No estás conectado al Master Server o no estás en el lobby. Espera a que la conexión se complete.");
            textIndicator.text = "Conectando... Por favor espera.";
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No se pudo unir a una sala aleatoria. Puedes crear una nueva sala si lo deseas.");
        // Aquí no creamos la sala automáticamente, dejamos que el usuario decida.
    }

    // Este método solo debe ser llamado cuando se presiona el botón "Crear Sala"
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

                // No enviar el RPC aquí, esperar hasta estar realmente en la sala
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

        // Ahora que estamos en la sala, podemos enviar el RPC
        photonView.RPC("RPC_SwitchWindow", RpcTarget.All, createRoomWindowIndex);
        UpdatePlayerListUI();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"Nuevo jugador unido a la sala: {newPlayer.NickName}");

        photonView.RPC("RPC_SwitchWindow", RpcTarget.All, createRoomWindowIndex);
        UpdatePlayerListUI();
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

    [PunRPC]
    public void RPC_SwitchWindow(int windowIndex)
    {
        EnabledWindow(windowIndex);
    }

    private void UpdatePlayerListUI()
    {
        if (playersContainer == null)
        {
            Debug.LogError("playersContainer no está asignado.");
            return;
        }

        if (playerNamePrefab == null)
        {
            Debug.LogError("playerNamePrefab no está asignado.");
            return;
        }

        foreach (Transform child in playersContainer)
        {
            Destroy(child.gameObject); // Limpia la lista actual
        }

        foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            GameObject playerNameObject = Instantiate(playerNamePrefab, playersContainer);
            TMP_Text playerNameText = playerNameObject.GetComponent<TMP_Text>();
            playerNameText.text = player.NickName;
        }
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
