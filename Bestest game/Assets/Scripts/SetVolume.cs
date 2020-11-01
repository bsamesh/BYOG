using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public enum Caller { MASTER,MUSIC,SFX};
    public Caller caller;

    public void SetLevel(float sliderValue)
    {
        if (caller == Caller.MASTER)
            AudioManager.masterSliderValue = sliderValue;
        else if (caller == Caller.MUSIC)
            AudioManager.musicSliderValue = sliderValue;
        else if (caller == Caller.SFX)
            AudioManager.sfxSliderValue = sliderValue;
        else
        {
            Debug.Log("Caller Error!");
            return;
        }

    }

    public void SetMasterVolume(float sliderValue)
    {
        mixer.SetFloat("masterVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("musicVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSfxVolume(float sliderValue)
    {
        mixer.SetFloat("sfxVol", Mathf.Log10(sliderValue) * 20);
    }

}
