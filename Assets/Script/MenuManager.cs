using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject optionsMenu;

    // Update is called once per frame
    void Update()
    {
        // Reverse the active state every time escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Check whether it's active / inactive 
            bool isActive = optionsMenu.activeSelf;

            optionsMenu.SetActive(!isActive);
        }
    }
}