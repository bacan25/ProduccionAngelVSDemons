using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image imageComponent;
    public Sprite newSprite;
    public int slotID;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
    }

    public void SetNewImage(bool isActive)
    {
        if (newSprite != null)
        {
            imageComponent.sprite = newSprite;
            imageComponent.enabled = isActive; // Activar o desactivar la imagen según el parámetro
        }
    }
}
