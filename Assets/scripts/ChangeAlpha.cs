using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAlpha : MonoBehaviour
{
    public RawImage doblesaltoIcon;
    public RawImage climbIcon;
    public RawImage powerUpIcon;
    private Color color;
    public RawImage coolDownIcon;


    // Función para cambiar el alpha de la imagen
    public void SetDobleSaltoIcon(float alpha)
    {
        
        color = doblesaltoIcon.color;
        color.a = Mathf.Clamp(alpha, 0f, 1f);
        doblesaltoIcon.color = color;
    }

    public void SetClimbIcon(float alpha)
    {

        color = climbIcon.color;
        color.a = Mathf.Clamp(alpha, 0f, 1f);
        climbIcon.color = color;
    }

    public void SetPowerUpIcon(float alpha)
    {

        color = powerUpIcon.color;
        color.a = Mathf.Clamp(alpha, 0f, 1f);
        powerUpIcon.color = color;
    }

    public void RedCoolDownIcon()
    {

        color = coolDownIcon.color;
        color.r = 255f;
        color.g = 0f;
        color.b = 0f;
        coolDownIcon.color = color;
    }

    public void GreenCoolDownIcon()
    {

        color = coolDownIcon.color;
        color.r = 0f;
        color.g = 191f;
        color.b = 17f;
        coolDownIcon.color = color;
    }



}
