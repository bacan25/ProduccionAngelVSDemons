using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercaderDetect : MonoBehaviour
{
    public GameObject mercaderText;
    public GameObject comprarPanel;

    private void Awake()
    {
        mercaderText = GameObject.Find("MercaderText");
        comprarPanel = GameObject.Find("ComprarPanel");
        mercaderText.gameObject.SetActive(false);
        comprarPanel.gameObject.SetActive(false);
    }

    void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Mercader"))
        {
            mercaderText.gameObject.SetActive(true);
            comprarPanel.gameObject.SetActive(true);

            if (Input.GetKey(KeyCode.Alpha1))
            {

            }

        }

    }

    // Este método se llama cuando otro Collider sale del rango (trigger)
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mercader"))
        {
            mercaderText.gameObject.SetActive(false);
            comprarPanel.gameObject.SetActive(false);

        }
    }
}
