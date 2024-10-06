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

    public void SetNewImage()
    {
        if (newSprite != null)
        {
            imageComponent.sprite = newSprite;
        }
    }
}
