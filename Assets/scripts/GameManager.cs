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
    public GameObject btnJoinRoom;  // Botón "Unirse a Sala"
    public GameObject btnCreateRoom;  // Botón "Crear Sala"
    public GameObject[] windows;
    public int createRoomWindowIndex;
    public int startWindowIndex;
    public GameObject playerNamePrefab;
    public Transform playersContainer;

    private int maxRooms = 10; // Solo pueden existir 10 salas
    private string baseRoomName = "Sala"; // Nombre base de las salas

    private void Start() {
        btnConect.SetActive(false);
        btnJoinRoom.SetActive(false); // Ocultar botón "Unirse a Sala" al inicio
        btnCreateRoom.SetActive(false); // Ocultar botón "Crear Sala" al inicio
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
        btnJoinRoom.SetActive(true); // Mostrar botón "Unirse a Sala"
        btnCreateRoom.SetActive(true); // Mostrar botón "Crear Sala"
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.LogError("Desconectado de Photon: " + cause.ToString());
        textIndicator.text = "Desconectado de Photon: " + cause.ToString();

        btnJoinRoom.SetActive(false); // Ocultar botón "Unirse a Sala" si se desconecta
        btnCreateRoom.SetActive(false); // Ocultar botón "Crear Sala" si se desconecta
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            StartCoroutine(TryJoinOrCreateRoom());
        }
        else
        {
            Debug.LogError("No estás conectado al Master Server. Espera a que la conexión se complete.");
            textIndicator.text = "Conectando... Por favor espera.";
        }
    }

    private IEnumerator TryJoinOrCreateRoom()
    {
        bool roomJoined = false;

        for (int i = 1; i <= maxRooms; i++)
        {
            string roomName = baseRoomName + i;

            PhotonNetwork.JoinRoom(roomName);
            Debug.Log("Intentando unirse a la sala: " + roomName);

            float timeout = 2.0f;
            float timer = 0;

            while (!roomJoined && timer < timeout)
            {
                if (PhotonNetwork.InRoom)
                {
                    roomJoined = true;
                    Debug.Log("Unido a la sala: " + roomName);
                    break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            if (roomJoined)
            {
                break;
            }
        }

        if (!roomJoined)
        {
            Debug.Log("Todas las salas están llenas o no existen. Creando una nueva sala...");
            CreateRoom();
        }
    }

    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            for (int i = 1; i <= maxRooms; i++)
            {
                string nameRoom = baseRoomName + i;

                RoomOptions optionRoom = new RoomOptions();
                optionRoom.IsVisible = true;
                optionRoom.IsOpen = true;
                optionRoom.MaxPlayers = 2;
                optionRoom.PublishUserId = true;

                PhotonNetwork.CreateRoom(nameRoom, optionRoom, TypedLobby.Default);
                Debug.Log("Creando sala: " + nameRoom);

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
        Debug.Log("Unido a la sala: " + PhotonNetwork.CurrentRoom.Name);

        EnabledWindow(createRoomWindowIndex);

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
        Debug.LogWarning("No se pudo unir a la sala. Intentando la siguiente sala...");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogError("Fallo al crear la sala: " + message);
        textIndicator.text = "Fallo al crear la sala: " + message;

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
