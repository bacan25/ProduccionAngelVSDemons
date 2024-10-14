using UnityEngine;
using System.Collections;

public class PvpFinal : MonoBehaviour
{
    [SerializeField] private Transform pos1; // El que llega primero
    [SerializeField] private Transform pos2; 

    private bool isFighting = false; 
    private GameObject player1; 
    private GameObject player2; 

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
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

            player1.transform.position = pos1.position;
            player2.transform.position = pos2.position;

            // Activa el pvp (Podemos desactivar el resto de cosas acá, hacer pruebas y poner parees invisibles)
            isFighting = true;
            StartCoroutine(VerificarVidaJugadores());
        }
        
    }

    public IEnumerator VerificarVidaJugadores()
    {
        HealthSystem health1 = player1.GetComponent<HealthSystem>();
        HealthSystem health2 = player2.GetComponent<HealthSystem>();

        while (isFighting)
        {
            if (health1.currentHealth <= 0) 
            {
                FightEnded(player2);
                yield break;
            }
            else if (health2.currentHealth <= 0) 
            {
                FightEnded(player1);
                yield break;
            }
            yield return new WaitForSeconds(2.0f);
        }
    }

    private void FightEnded(GameObject ganador)
    {
        isFighting = false;
        Debug.Log(ganador.name + " humilló a su oponente. Mejoren bots.");
    }
}
