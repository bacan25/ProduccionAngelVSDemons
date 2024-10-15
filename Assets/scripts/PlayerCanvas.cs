using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    public static PlayerCanvas Instance; // Singleton
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;

    [SerializeField] private Image basicAbilityCooldownImage;
    [SerializeField] private Image powerAbilityCooldownImage;

    [SerializeField] private Text monedasText;
    [SerializeField] private Text pocionesText;

    public int monedasPlayer;
    private int cantidadPociones;

    private void Awake()
    {
        // Configuración del singleton
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
        if (healthBar != null)
        {
            healthBar.value = healthPercent * 100;  // Multiplica por 100 para ajustarlo al slider
        }
        else
        {
            Debug.LogError("Barra de vida no asignada en el PlayerCanvas.");
        }
    }

    public void UpdateAbilityCooldown(string abilityName, float cooldownPercent)
    {
        if (abilityName == "BasicAttack" && basicAbilityCooldownImage != null)
        {
            basicAbilityCooldownImage.fillAmount = 1 - cooldownPercent;
        }
        else if (abilityName == "PowerAttack" && powerAbilityCooldownImage != null)
        {
            powerAbilityCooldownImage.fillAmount = 1 - cooldownPercent;
        }
        else
        {
            Debug.LogError($"La habilidad {abilityName} no tiene una imagen de enfriamiento asignada o no existe.");
        }
    }

    public void UnlockAbility(string abilityName)
    {
        Debug.Log($"Habilidad desbloqueada: {abilityName}");
    }

    public void LockAbility(string abilityName)
    {
        Debug.Log($"Habilidad bloqueada: {abilityName}");
    }

    public void WinCanvas()
    {
        winCanvas.SetActive(true);
    }

    public void LoseCanvas()
    {
        loseCanvas.SetActive(true);
    }

    public void UpdateGoldDisplay(int newGoldAmount)
    {
        if (monedasText != null)
        {
            monedasText.text = newGoldAmount.ToString();
        }
        else
        {
            Debug.LogError("Texto de monedas no asignado en el PlayerCanvas.");
        }
    }

    public void SumarPociones()
    {
        cantidadPociones++;
        UpdatePocionesDisplay();
    }

    public void RestarPociones()
    {
        if (cantidadPociones > 0)
        {
            cantidadPociones--;
            UpdatePocionesDisplay();
        }
        else
        {
            Debug.LogWarning("No hay más pociones para restar.");
        }
    }

    private void UpdatePocionesDisplay()
    {
        if (pocionesText != null)
        {
            pocionesText.text = cantidadPociones.ToString();
        }
        else
        {
            Debug.LogError("Texto de pociones no asignado en el PlayerCanvas.");
        }
    }
}
