using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class GameManagerFPS : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    public TMP_Text resultText;  // Cambiado a TMP_Text
    public Slider player1HealthBar;
    public Slider player2HealthBar;

    private PlayerController player1;
    private PlayerController player2;

    void Start()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            SpawnPlayer();
        }

        // Asegúrate de que el texto de resultado esté deshabilitado al inicio
        resultText.gameObject.SetActive(false);
    }

    void SpawnPlayer()
    {
        Transform spawnPoint = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1];

        // Verifica que playerPrefab no sea nulo antes de instanciarlo
        if (playerPrefab != null)
        {
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);

            PlayerController playerController = player.GetComponent<PlayerController>();
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            {
                player1 = playerController;
                player1.SetHealthBar(player1HealthBar);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
            {
                player2 = playerController;
                player2.SetHealthBar(player2HealthBar);
            }
        }
        else
        {
            Debug.LogError("Player prefab is not assigned or found!");
        }
    }


    public void CheckForWinner()
    {
        if (player1 != null && player1.IsDead())
        {
            photonView.RPC("DeclareWinner", RpcTarget.All, 2);
        }
        else if (player2 != null && player2.IsDead())
        {
            photonView.RPC("DeclareWinner", RpcTarget.All, 1);
        }
    }

    [PunRPC]
    void DeclareWinner(int winnerPlayerNumber)
    {
        // Habilita el texto de resultado cuando se declara un ganador
        resultText.gameObject.SetActive(true);

        if (PhotonNetwork.LocalPlayer.ActorNumber == winnerPlayerNumber)
        {
            resultText.text = "You Win!";
            resultText.color = Color.green;  // Cambia el color o estilo según prefieras
        }
        else
        {
            resultText.text = "You Lose!";
            resultText.color = Color.red;  // Cambia el color o estilo según prefieras
        }
    }
}
