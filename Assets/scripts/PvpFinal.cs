using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon.Pun;

public class PvpFinal : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform pos1; // El que llega primero
    [SerializeField] private Transform pos2;

    private bool isFighting = false;
    private GameObject player1;
    private GameObject player2;
    [SerializeField]private GameObject paredesFinal;
    PlayerCanvas playerCanvas;

    void VueltaInicio()
    {
        SceneManager.LoadScene(0);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            paredesFinal.SetActive(true);
            this.transform.position += new Vector3(0,100,0);
            // Verificar si ya comenzó el combate
            if (isFighting) return;

            // Asegurarse de que InGameManager está inicializado
            InGameManager gameManager = InGameManager.Instance;
            if (gameManager == null)
            {
                Debug.LogError("InGameManager no está inicializado. Asegúrate de que está en la escena y correctamente configurado.");
                return;
            }

            player1 = col.gameObject;

            // Busca al otro jugador
            player2 = FindOtherPlayer(gameManager);
            if (player2 == null)
            {
                Debug.LogWarning("No se encontró al otro jugador. Esperando a que ambos jugadores estén presentes.");
                return;
            }

            // Enviar una llamada RPC a todos los jugadores para iniciar el combate
            photonView.RPC("StartFight", RpcTarget.AllBuffered, player1.GetComponent<PhotonView>().ViewID, player2.GetComponent<PhotonView>().ViewID);
            
        }
    }

    private GameObject FindOtherPlayer(InGameManager gameManager)
    {
        // Busca al otro jugador en la lista del InGameManager
        foreach (Transform playerTransform in gameManager.playerTransforms)
        {
            GameObject p = playerTransform.gameObject;
            if (p != player1)
            {
                return p;
            }
        }
        return null; // Si no encuentra otro jugador, devuelve null
    }

    [PunRPC]
    public void StartFight(int player1ID, int player2ID)
    {
        
        // Encontrar a los jugadores por su ViewID
        player1 = PhotonView.Find(player1ID)?.gameObject;
        player2 = PhotonView.Find(player2ID)?.gameObject;

        // Asegurarse de que ambos jugadores fueron encontrados antes de proceder
        if (player1 == null || player2 == null)
        {
            Debug.LogError("Uno o ambos jugadores no fueron encontrados para el combate.");
            return;
        }

        // Posicionar a ambos jugadores en las posiciones establecidas
        player1.transform.position = pos1.position;
        player2.transform.position = pos2.position;

        // Activa el PvP
        isFighting = true;
        
        StartCoroutine(VerificarVidaJugadores());
    }

    public IEnumerator VerificarVidaJugadores()
    {
        // Asegurarse de que ambos jugadores y sus sistemas de vida existen
        if (player1 == null || player2 == null)
        {
            Debug.LogError("Uno o ambos jugadores no están presentes al iniciar la verificación de vida.");
            yield break;
        }

        HealthSystem health1 = player1.GetComponent<HealthSystem>();
        HealthSystem health2 = player2.GetComponent<HealthSystem>();

        if (health1 == null || health2 == null)
        {
            Debug.LogError("No se pudo encontrar el HealthSystem en uno o ambos jugadores.");
            yield break;
        }

        while (isFighting)
        {
            if (health1.currentHealth <= 0)
            {
                photonView.RPC("FightEnded", RpcTarget.AllBuffered, player2.GetComponent<PhotonView>().ViewID, player1.GetComponent<PhotonView>().ViewID);
                yield break;
            }
            else if (health2.currentHealth <= 0)
            {
                photonView.RPC("FightEnded", RpcTarget.AllBuffered, player1.GetComponent<PhotonView>().ViewID, player2.GetComponent<PhotonView>().ViewID);
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    //Hacer que le salga a cada uno un canvas diferente
    [PunRPC]
    private void FightEnded(int winnerID, int loserID)
    {
        isFighting = false;
        PlayerCanvas playerCanvas = PlayerCanvas.Instance;
        GameObject ganador = PhotonView.Find(winnerID)?.gameObject;
        GameObject perdedor = PhotonView.Find(loserID)?.gameObject;

        // Asegurarse de que el ganador existe antes de proceder
        if (ganador != null)
        {
            if(ganador.GetComponent<PhotonView>().IsMine)
            {
                Debug.Log("Gané");
                playerCanvas.WinCanvas();
                Invoke("VueltaInicio",4f);
            }
            else if(perdedor.GetComponent<PhotonView>().IsMine)
            {
                Debug.Log("Perdí");
                playerCanvas.LoseCanvas();
                Invoke("VueltaInicio",4f);
            }
        }
        else
        {
            Debug.LogError("El jugador ganador no fue encontrado.");
        }
    }
}
