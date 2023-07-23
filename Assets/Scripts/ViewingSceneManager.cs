using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewingSceneManager : MonoBehaviour
{
    public void NextSceneWithString()
    {
        SceneManager.LoadScene("ViewingMode");
    }
}
