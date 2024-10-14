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
    public bool enablePanel;
    public bool insideRange;

    private void Start()
    {
        // Agregamos o accedemos al SphereCollider
        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true; // Lo hacemos un trigger
        sphereCollider.radius = range;   // Definimos el radio del rango
        enablePanel = false;
    }

    void OnDrawGizmos()
    {
        // Cambiamos el color del Gizmo
        Gizmos.color = gizmoColor;

        // Dibujamos una esfera alrededor del objeto, usando su posición y el rango
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void Update()
    {
        

    }

    // Este método se llama cuando otro Collider entra en el rango (trigger)
    void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            mercaderText.gameObject.SetActive(true);
            comprarPanel.gameObject.SetActive(true);

        }
            
    }

    // Este método se llama cuando otro Collider sale del rango (trigger)
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mercaderText.gameObject.SetActive(false);
            comprarPanel.gameObject.SetActive(false);
            
        }
    }

    
}
