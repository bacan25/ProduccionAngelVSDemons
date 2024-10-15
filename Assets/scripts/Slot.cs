using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image imageComponent;
    public Sprite baseSprite;     // Imagen base cuando el objeto no está adquirido
    public Sprite acquiredSprite; // Imagen cuando el objeto ha sido adquirido
    public int slotID;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
    }

    public void SetNewImage(bool isActive)
    {
        if (isActive && acquiredSprite != null)
        {
            imageComponent.sprite = acquiredSprite; // Imagen cuando el objeto está adquirido
        }
        else if (!isActive && baseSprite != null)
        {
            imageComponent.sprite = baseSprite; // Imagen base cuando el objeto no está adquirido
        }
    }
}
