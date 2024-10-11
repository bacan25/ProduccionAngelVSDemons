using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    public static PlayerCanvas Instance; // Singleton
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;
    

    // UI para habilidades (esto podría ser una lista de imágenes o barras)
    // Aquí supongo que tienes imágenes para representar habilidades y su enfriamiento.
    [SerializeField] private Image basicAbilityCooldownImage;
    [SerializeField] private Image powerAbilityCooldownImage;

    [SerializeField] private Text monedasText;
    public int monedas;
    [SerializeField] private Text pocionesText;
    public int pociones;

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


    // Método para actualizar la visualización del enfriamiento de una habilidad
    public void UpdateAbilityCooldown(string abilityName, float cooldownPercent)
    {
        // Asegúrate de actualizar la habilidad correcta en función del nombre de la habilidad
        if (abilityName == "BasicAttack" && basicAbilityCooldownImage != null)
        {
            basicAbilityCooldownImage.fillAmount = 1 - cooldownPercent; // Inverso para llenar la imagen correctamente
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
        // Aquí puedes agregar la lógica para actualizar la UI con la habilidad desbloqueada.
    }

    public void LockAbility(string abilityName)
    {
        Debug.Log($"Habilidad bloqueada: {abilityName}");
        // Aquí puedes agregar la lógica para bloquear la habilidad en la UI.
    }

    public void WinCanvas()
    {
        winCanvas.SetActive(true);
    }

    public void LoseCanvas()
    {
        loseCanvas.SetActive(true);
    }

    public void SumarMonedas()
    {
        monedas += 10;
        monedasText.text = monedas.ToString();
    }

    public void RestarMonedas()
    {
        monedas -= 15;
        monedasText.text = monedas.ToString();
    }

    public void SumarPociones()
    {
        pociones += 1;
        pocionesText.text = pociones.ToString();
    }

    public void RestarPociones()
    {
        pociones -= 1;
        pocionesText.text = pociones.ToString();
    }
}
