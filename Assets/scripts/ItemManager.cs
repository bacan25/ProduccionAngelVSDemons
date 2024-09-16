using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public GameObject activeItem = null;
    public int itemID;
    public string itemType;
    public int slotInum;
    public Text potionTexto;

    public Health health; // Asegúrate de que está asignado o lo buscaremos en Start()
    public Inventory clearSlot; // Asegúrate de que esté asignado en el inspector
    public AngelClass shootVel; // Asegúrate de que esté asignado

    private void Start()
    {
        // Verifica que el componente Health esté asignado, si no, lo buscamos en el mismo GameObject
        if (health == null)
        {
            health = GetComponent<Health>();
            if (health == null)
            {
                Debug.LogError("El componente Health no está asignado y no se encontró en el GameObject.");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) && itemType == "accesory")
        {
            Accesory();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (itemID == 1)
            {
                Potion();
            }
            else
            {
                AutoPotion();
            }
        }
    }

    private void Potion()
    {
        if (health != null)
        {
            UsePotion();
            clearSlot.ClearOtherSlot(); // Asegúrate de que clearSlot esté asignado correctamente
            itemID = 0;
        }
        else
        {
            Debug.LogError("El componente Health no está asignado.");
        }
    }

    public void AutoPotion()
    {
        if (clearSlot != null && clearSlot.potionNum > 0)
        {
            UsePotion();
            clearSlot.AutoClearPotionSlot();
        }
    }

    private void UsePotion()
    {
        if (health != null)
        {
            health.Potion(); // Llamar al método Potion del script Health
            clearSlot.potionNum -= 1;
            potionTexto.text = clearSlot.potionNum.ToString();

            if (health.currentHealth > health.maxHealth)
            {
                health.currentHealth = health.maxHealth;
            }

            if (activeItem != null && itemID == 1)
            {
                activeItem.SetActive(false);
                activeItem = null;
            }
        }
    }

    private void Accesory()
    {
        if (itemID == 2 && shootVel != null)
        {
            Debug.Log("UseAcccesory");
            shootVel.basicCooldown = 0.3f;
            clearSlot.ClearOtherSlot();

            if (activeItem != null)
            {
                activeItem.SetActive(false);
                activeItem = null;
            }
        }
        itemID = 0;
    }
}
