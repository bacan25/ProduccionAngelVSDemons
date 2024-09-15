using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] private GameObject winCanvas; 
    [SerializeField] private GameObject loseCanvas; 
   
    public void WinCanvas()
    {
        //Poner que se desactiva todo xd
        winCanvas.SetActive(true);
        
    }
    public void LoseCanvas()
    {  
        //Poner que se desactiva todo xd
        loseCanvas.SetActive(true);
        
    }
}
