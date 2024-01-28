using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelName;
    public Transform panel;
    public Button optionOpenButton;
    public Button optionReturnButton;
    public Slider musicSlider;
    public Slider sfxSlider;

    public SoundManager soundManager;
    public AudioClip menuMusic;

    // Start is called before the first frame update
    void Start()
    {
        soundManager.PlayNewClip(menuMusic);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleOptions(bool value)
    {
        panel.gameObject.SetActive(value);
        if (value)
        {
            optionReturnButton.Select();
        }
        else
        {
            optionOpenButton.Select();
        }
    }

    public void UpdateMusicVolume()
    {
        GameManager.musicVolume = musicSlider.value;
        soundManager.UpdateVolume();
    }
}
