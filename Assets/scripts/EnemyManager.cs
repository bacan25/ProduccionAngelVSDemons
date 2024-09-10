using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Lista de enemigos en el campamento
    public List<EnemyAI> enemiesInCamp = new List<EnemyAI>();

    // Transform del jugador detectado
    public Transform playerDetected = null;

    void Start()
    {
        // Asegúrate de que la lista de enemigos está llena
        foreach (EnemyAI enemy in enemiesInCamp)
        {
            enemy.SetEnemyManager(this); // Le pasamos la referencia de este EnemyManager a cada enemigo
        }
    }

    // Método para notificar a todos los enemigos cuando uno detecta al jugador
    public void NotifyPlayerDetected(Transform player)
    {
        playerDetected = player;

        // Informar a todos los enemigos del campamento
        foreach (EnemyAI enemy in enemiesInCamp)
        {
            enemy.OnPlayerDetected(player);
        }
    }

    // Método para reiniciar la detección si el jugador ya no es detectado
    public void ResetDetection()
    {
        playerDetected = null;
    }
}
