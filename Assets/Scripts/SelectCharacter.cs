using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    public Character character;
    Animator anim;
    public SelectCharacter[] characters;
    void Start()
    {
        anim = GetComponent<Animator>();
        if (DataManager.instance.currentCharacter == character) OnSelect();
        else OnDeSelect();
    }
    private void OnMouseUpAsButton()
    {
        DataManager.instance.currentCharacter = character;
        OnSelect();
        for(int i=0; i<characters.Length; i++)
        {
            if (characters[i] != this) characters[i].OnDeSelect();
        }
    }
    void OnDeSelect()
    {
        anim.SetBool("Select", false);
    }
    void OnSelect()
    {
        anim.SetBool("Select", true);
    }
}
