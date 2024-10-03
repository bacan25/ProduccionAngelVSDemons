using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            // Obtener el componente HealthSystem del jugador y aplicar daño por caída
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.TakeFallDamage(); // Aplicar daño por caída que mata al jugador
            }
        }
    }
}
