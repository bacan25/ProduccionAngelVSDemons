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

    [SerializeField] private Text monedasText; // Texto que muestra la cantidad de monedas
    [SerializeField] private Text pocionesText; // Texto que muestra la cantidad de pociones
    public ItemManager counter;

    public int monedasPlayer;

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

    public void SumarMonedas(int cantidad)
    {
        monedasPlayer += cantidad;
        UpdateGoldDisplay(monedasPlayer); // Llama a la actualización del texto de las monedas
    }

    public void RestarMonedas(int cantidad)
    {
        if (monedasPlayer >= cantidad)
        {
            monedasPlayer -= cantidad;
            UpdateGoldDisplay(monedasPlayer);
        }
        else
        {
            Debug.LogWarning("No hay suficientes monedas para restar.");
        }
    }

    public void SumarPociones()
    {
        if (pocionesText != null && counter != null)
        {
            pocionesText.text = counter.pociones.ToString();
        }
        else
        {
            Debug.LogError("Texto de pociones o ItemManager no asignado en el PlayerCanvas.");
        }
    }

    public void RestarPociones()
    {
        if (pocionesText != null && counter != null)
        {
            pocionesText.text = counter.pociones.ToString();
        }
        else
        {
            Debug.LogError("Texto de pociones o ItemManager no asignado en el PlayerCanvas.");
        }
    }

    // Cambiado a public para que pueda ser llamado desde otros scripts
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
}
