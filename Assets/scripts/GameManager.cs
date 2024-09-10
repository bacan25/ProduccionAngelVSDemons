using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField nicknameInputField;
    public GameObject startButton;
    public GameObject readyButton;
    public GameObject readyIndicator;
    public TMP_Text player1NameText;
    public TMP_Text player2NameText;

    private bool isPlayerReady = false;

    // Lista para almacenar los Transforms de los jugadores
    public List<Transform> playerTransforms = new List<Transform>();

    private void Start()
    {
        startButton.SetActive(true);
        readyButton.SetActive(false);
    }

    public void SetNicknameAndConnect()
    {
        string nickname = nicknameInputField.text;
        if (!string.IsNullOrEmpty(nickname))
        {
            PhotonNetwork.NickName = nickname;
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log($"Nombre del jugador establecido: {nickname}");
            startButton.SetActive(false);
        }
        else
        {
            Debug.LogWarning("El nickname no puede estar vacío.");
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Unido a la sala: " + PhotonNetwork.CurrentRoom.Name);
        UpdatePlayerListUI();
        readyButton.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Jugador {newPlayer.NickName} ha entrado a la sala.");
        UpdatePlayerListUI(); // Asegura que la lista de jugadores se actualice cuando un nuevo jugador entra
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Jugador {otherPlayer.NickName} ha dejado la sala.");
        UpdatePlayerListUI(); // Actualizar la lista de jugadores
        readyIndicator.GetComponent<TMP_Text>().text = "Waiting for a player to join...";
        readyButton.SetActive(false); // Desactiva el botón Ready hasta que un nuevo jugador entre
    }

    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public void ToggleReady()
    {
        isPlayerReady = !isPlayerReady;
        photonView.RPC("UpdateReadyState", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber, isPlayerReady);
    }

    [PunRPC]
    private void UpdateReadyState(int playerID, bool isReady)
    {
        PhotonNetwork.CurrentRoom.GetPlayer(playerID).SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "IsReady", isReady } });
        CheckAllPlayersReady();
    }

    private void UpdatePlayerListUI()
    {
        player1NameText.text = "";
        player2NameText.text = "";

        var players = PhotonNetwork.CurrentRoom.Players.Values;
        int index = 0;

        foreach (Player player in players)
        {
            if (index == 0)
            {
                player1NameText.text = player.NickName;
            }
            else if (index == 1)
            {
                player2NameText.text = player.NickName;
            }
            index++;
        }
    }

    private void CheckAllPlayersReady()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            readyIndicator.GetComponent<TMP_Text>().text = "Waiting for a player to join...";
            return;
        }

        int notReadyCount = 0;

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (!player.CustomProperties.ContainsKey("IsReady") || !(bool)player.CustomProperties["IsReady"])
            {
                notReadyCount++;
            }
        }

        if (notReadyCount == 0)
        {
            StartCoroutine(StartGameCountdown());
        }
        else
        {
            readyIndicator.GetComponent<TMP_Text>().text = $"Waiting for {notReadyCount} player(s) to be ready...";
        }
    }

    private IEnumerator StartGameCountdown()
    {
        readyIndicator.SetActive(false);
        yield return new WaitForSeconds(5);

        PhotonNetwork.LoadLevel("Valentina");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Debug.Log("Saliendo de la sala...");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Has salido de la sala.");
        startButton.SetActive(true);
        readyButton.SetActive(false);
        readyIndicator.GetComponent<TMP_Text>().text = "";
        player1NameText.text = "";
        player2NameText.text = "";
    }

    // Método para que los jugadores se registren en el GameManager
    public void RegisterPlayer(Transform playerTransform)
    {
        if (!playerTransforms.Contains(playerTransform))
        {
            playerTransforms.Add(playerTransform);
        }
    }
}
