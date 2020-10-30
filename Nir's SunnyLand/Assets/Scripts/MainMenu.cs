using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer Mixer;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public TextMeshProUGUI masterPercentageText;
    public TextMeshProUGUI musicPercentageText;
    public TextMeshProUGUI sfxPercentageText;

    void Start()
    {
        FindObjectOfType<AudioManager>().StopPlayingEverything();
        FindObjectOfType<AudioManager>().Play("MenuTheme");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        FindObjectOfType<AudioManager>().StopPlayingEverything();
        FindObjectOfType<AudioManager>().Play("Theme1");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void OptionsGame()
    {
        masterSlider.value = AudioManager.masterSliderValue;
        masterPercentageText.text = Mathf.RoundToInt(AudioManager.masterSliderValue * 100) + "%";
        musicSlider.value = AudioManager.musicSliderValue;
        musicPercentageText.text = Mathf.RoundToInt(AudioManager.musicSliderValue * 100) + "%";
        sfxSlider.value = AudioManager.sfxSliderValue;
        sfxPercentageText.text = Mathf.RoundToInt(AudioManager.sfxSliderValue * 100) + "%";
    }
}
