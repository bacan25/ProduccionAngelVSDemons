using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    public static PlayerCanvas Instance; // Singleton
    [SerializeField] private Slider healthBar; 
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;

    private void Awake()
    {
        // Implementación del Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados
        }
    }

    public void UpdateHealthBar(float healthPercent)
    {
        healthBar.value = healthPercent;
    }

    public void UnlockAbility(string abilityName)
    {
        Debug.Log($"Habilidad desbloqueada: {abilityName}");
        // Actualiza la UI para mostrar la habilidad desbloqueada
    }

    public void LockAbility(string abilityName)
    {
        Debug.Log($"Habilidad bloqueada: {abilityName}");
        // Actualiza la UI para bloquear la habilidad
    }

    public void UpdateAbilityCooldown(string abilityName, float cooldownPercent)
    {
        Debug.Log($"Cooldown de {abilityName}: {cooldownPercent * 100}%");
        // Actualiza visualmente el cooldown de la habilidad en la UI
    }

    public void WinCanvas()
    {
        winCanvas.SetActive(true);
    }

    public void LoseCanvas()
    {
        loseCanvas.SetActive(true);
    }
}
