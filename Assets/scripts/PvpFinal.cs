using UnityEngine;
using System.Collections;
using Photon.Pun;

public class PvpFinal : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform pos1; // El que llega primero
    [SerializeField] private Transform pos2;

    private bool isFighting = false;
    private GameObject player1;
    private GameObject player2;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // Verificar si ya comenzó el combate
            if (isFighting) return;

            InGameManager gameManager = InGameManager.Instance;

            player1 = col.gameObject;

            // Busca al otro jugador
            foreach (Transform playerTransform in gameManager.playerTransforms)
            {
                GameObject p = playerTransform.gameObject;
                if (p != player1)
                {
                    player2 = p;
                    break;
                }
            }

            if (player2 != null)
            {
                // Enviar una llamada RPC a todos los jugadores para iniciar el combate
                photonView.RPC("StartFight", RpcTarget.AllBuffered, player1.GetComponent<PhotonView>().ViewID, player2.GetComponent<PhotonView>().ViewID);
            }
        }
    }

    [PunRPC]
    public void StartFight(int player1ID, int player2ID)
    {
        player1 = PhotonView.Find(player1ID).gameObject;
        player2 = PhotonView.Find(player2ID).gameObject;

        // Posicionar a ambos jugadores en las posiciones establecidas
        player1.transform.position = pos1.position;
        player2.transform.position = pos2.position;

        // Activa el pvp
        isFighting = true;

        StartCoroutine(VerificarVidaJugadores());
    }

    public IEnumerator VerificarVidaJugadores()
    {
        HealthSystem health1 = player1.GetComponent<HealthSystem>();
        HealthSystem health2 = player2.GetComponent<HealthSystem>();

        while (isFighting)
        {
            if (health1.currentHealth <= 0)
            {
                photonView.RPC("FightEnded", RpcTarget.AllBuffered, player2.GetComponent<PhotonView>().ViewID);
                yield break;
            }
            else if (health2.currentHealth <= 0)
            {
                photonView.RPC("FightEnded", RpcTarget.AllBuffered, player1.GetComponent<PhotonView>().ViewID);
                yield break;
            }
            yield return new WaitForSeconds(2.0f);
        }
    }

    [PunRPC]
    private void FightEnded(int winnerID)
    {
        isFighting = false;

        GameObject ganador = PhotonView.Find(winnerID).gameObject;
        Debug.Log(ganador.name + " humilló a su oponente. Mejoren bots.");

        // Aquí podrías agregar lógica para manejar lo que sucede después del combate (reaparecer, recompensas, etc.)
    }
}
