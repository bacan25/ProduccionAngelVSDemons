using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class AbilityIcon
{
    public string abilityName;
    public Image icon;
}

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("Health UI")]
    public Slider healthBar;

    [Header("Inventory UI")]
    public GameObject inventoryUI;
    public Text potionCountText;

    [Header("Ability UI")]
    public List<AbilityIcon> abilityIconsList = new List<AbilityIcon>();
    private Dictionary<string, Image> abilityIcons = new Dictionary<string, Image>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Si deseas que persista entre escenas, puedes usar DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Inicializar el diccionario de habilidades
        foreach (var abilityIcon in abilityIconsList)
        {
            if (!abilityIcons.ContainsKey(abilityIcon.abilityName))
            {
                abilityIcons.Add(abilityIcon.abilityName, abilityIcon.icon);
            }
        }
    }

    // Método para actualizar la barra de vida
    public void UpdateHealth(float healthPercent)
    {
        if (healthBar != null)
        {
            healthBar.value = healthPercent;
        }
    }

    // Método para mostrar/ocultar el inventario
    public void ToggleInventory(bool isActive)
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(isActive);
        }
    }

    // Métodos para actualizar las habilidades
    public void UpdateAbilityCooldown(string abilityName, float cooldownPercent)
    {
        if (abilityIcons.ContainsKey(abilityName))
        {
            abilityIcons[abilityName].fillAmount = cooldownPercent;
        }
    }

    public void UnlockAbility(string abilityName)
    {
        if (abilityIcons.ContainsKey(abilityName))
        {
            // Cambiar la apariencia del ícono para indicar que está desbloqueado
            Color iconColor = abilityIcons[abilityName].color;
            iconColor.a = 1f; // Hacer el ícono completamente visible
            abilityIcons[abilityName].color = iconColor;
        }
    }

    public void LockAbility(string abilityName)
    {
        if (abilityIcons.ContainsKey(abilityName))
        {
            // Cambiar la apariencia del ícono para indicar que está bloqueado
            Color iconColor = abilityIcons[abilityName].color;
            iconColor.a = 0.5f; // Hacer el ícono semi-transparente
            abilityIcons[abilityName].color = iconColor;
        }
    }

    // Método para actualizar el conteo de pociones
    public void UpdatePotionCount(int count)
    {
        if (potionCountText != null)
        {
            potionCountText.text = count.ToString();
        }
    }
}
