using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Transform playerDetected = null; // Referencia al jugador detectado

    void Update()
    {
        // Aquí se pueden manejar otros comportamientos basados en el jugador detectado
        if (playerDetected != null)
        {
            Debug.Log("Jugador detectado: " + playerDetected.name);
        }
    }
}
