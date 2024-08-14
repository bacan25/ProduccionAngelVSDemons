using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowHandler : MonoBehaviour
{
    public GameObject[] windows;

    public void EnabledWindow(int idWindow)
    {
        windows[idWindow].SetActive(true);

        for (int i = 0; i < windows.Length; i++)
        {
            if (idWindow != i)
                windows[i].SetActive(false);
        }
    }
}