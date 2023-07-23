using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void GameExit()
    {
        Application.Quit();
    }

    public void Logout()
    {
        SceneManager.LoadScene("Login");
    }
}
