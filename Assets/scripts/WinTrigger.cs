using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private bool gameEnded = false;
    public InGameManager inGameManager; 

    private void OnTriggerEnter(Collider col)
    { 
        if (!gameEnded && col.CompareTag("Player"))
        {
            gameEnded = true;
            PlayerCanvas winner = col.GetComponent<PlayerCanvas>();
            inGameManager.HandleWin(winner); 

        }
    }
}
