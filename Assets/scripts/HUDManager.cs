using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("Health UI")]
    public Slider healthBar;

    [Header("Inventory UI")]
    public GameObject inventoryUI;
    public Text potionCountText;

    [Header("Ability UI")]
    public Image basicAttackIcon;
    public Image powerAttackIcon;

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
    }

    // Métodos para actualizar la barra de vida
    public void UpdateHealth(float healthPercent)
    {
        if (healthBar != null)
        {
            healthBar.value = healthPercent;
        }
    }

    // Métodos para mostrar/ocultar el inventario
    public void ToggleInventory(bool isActive)
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(isActive);
        }
    }

    // Métodos para actualizar las habilidades
    public void UpdateBasicAttackCooldown(float cooldownPercent)
    {
        if (basicAttackIcon != null)
        {
            basicAttackIcon.fillAmount = cooldownPercent;
        }
    }

    public void UpdatePowerAttackCooldown(float cooldownPercent)
    {
        if (powerAttackIcon != null)
        {
            powerAttackIcon.fillAmount = cooldownPercent;
        }
    }

    // Actualizar el conteo de pociones
    public void UpdatePotionCount(int count)
    {
        if (potionCountText != null)
        {
            potionCountText.text = count.ToString();
        }
    }
}
