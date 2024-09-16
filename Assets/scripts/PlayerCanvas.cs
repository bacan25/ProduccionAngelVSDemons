using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] private GameObject winCanvas; 
    [SerializeField] private GameObject loseCanvas;
    public Move_Player player; 
   
    public void WinCanvas()
    {
        //Poner que se desactiva todo xd
        winCanvas.SetActive(true);
        player.muerto = true;
        
    }
    public void LoseCanvas()
    {  
        //Poner que se desactiva todo xd
        loseCanvas.SetActive(true);
        player.muerto = true;

    }
}
