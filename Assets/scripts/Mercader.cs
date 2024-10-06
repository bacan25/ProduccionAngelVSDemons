using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Mercader : MonoBehaviour
{
    public float range = 5f;  // Define el rango que quieres mostrar alrededor del objeto
    public Color gizmoColor = Color.green;  // Color del rango
    public GameObject mercaderText;
    public GameObject comprarPanel;
    private bool enablePanel;

    private void Start()
    {
        // Agregamos o accedemos al SphereCollider
        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true; // Lo hacemos un trigger
        sphereCollider.radius = range;   // Definimos el radio del rango
    }

    void OnDrawGizmos()
    {
        // Cambiamos el color del Gizmo
        Gizmos.color = gizmoColor;

        // Dibujamos una esfera alrededor del objeto, usando su posición y el rango
        Gizmos.DrawWireSphere(transform.position, range);
    }

    // Este método se llama cuando otro Collider entra en el rango (trigger)
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mercaderText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.C)) 
            { 
                enablePanel = !enablePanel;
                EnablePanel();
            }

            
        }
            
    }

    // Este método se llama cuando otro Collider sale del rango (trigger)
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mercaderText.gameObject.SetActive(false);
            enablePanel = false;
            EnablePanel();
        }
    }

    public void EnablePanel()
    {
        if (enablePanel)
        {
            comprarPanel.gameObject.SetActive(true);
        }
        else if (!enablePanel)
        {
            comprarPanel.gameObject.SetActive(false);
        }
    }
}
