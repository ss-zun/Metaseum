using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwitchMusicTrigger : MonoBehaviour
{

    public AudioClip newTrack1;
    public AudioClip newTrack2;
    public AudioClip newTrack3;

    private AudioManager theAM;
    private int trackIndex = 0;
    public Slider bgmVolumeSlider; // Slider UI 요소

    // Start is called before the first frame update
    void Start()
    {
        theAM = FindObjectOfType<AudioManager>();
        bgmVolumeSlider.value = theAM.GetBGMVolume(); // 초기화:슬라이더의 값 설정
    }

    public void SwitchBGMButton()
    {
        if (trackIndex == 0)
        {
            theAM.changeBGM(newTrack1);
            trackIndex++;
        }
        else if (trackIndex == 1)
        {
            theAM.changeBGM(newTrack2);
            trackIndex++;
        }
        else if (trackIndex == 2)
        {
            theAM.changeBGM(newTrack3);
            trackIndex = 0;
        }
    }
    public void OnBGMVolumeChanged(float value)
    {
        theAM.SetBGMVolume(value); // AudioManager에서 BGM 불륨 조절
    }
}

