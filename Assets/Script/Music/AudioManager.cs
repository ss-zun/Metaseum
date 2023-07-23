using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource BGM;
    public void changeBGM(AudioClip music)
    {
        BGM.Stop();
        BGM.clip = music;
        BGM.Play();
    }
    public void SetBGMVolume(float value)
    {
        BGM.volume = value;
    }
    public float GetBGMVolume()
    {
        return BGM.volume;
    }
}

