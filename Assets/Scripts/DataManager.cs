using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Character
{
    F_1, F_2, F_3, F_4, M_1, M_2, M_3, M_4
}
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public TMP_InputField inputNickName;
    public TMP_InputField inputRoomName;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) return;
        DontDestroyOnLoad(gameObject);
    }
    public Character currentCharacter;
}
