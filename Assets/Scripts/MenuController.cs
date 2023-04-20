using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public GameObject settingsScreen;
    public GameObject menuScreen;
    public TMP_Text musicOption;

    public TMP_Text sliderText;

    private int gridSize;

    public Slider slider;

    private bool isMusicEnabled;

    private void Start() {
        LoadGameDataSettings();
        if (settingsScreen != null) {
            settingsScreen.SetActive(false);
        }
    }

    private void LoadGameDataSettings() {
       GameData gameData = FileHandler.LoadGameData();
       isMusicEnabled = gameData.isMusicEnabled;
       gridSize = gameData.gridSizeSetting;
    }

    public void ChangeMusic() {
        isMusicEnabled = !isMusicEnabled;
        ChangeMusicButtonText();
        FileHandler.SaveMusicSetting(isMusicEnabled);
    }
    
    private void ChangeMusicButtonText() {
        if (isMusicEnabled) {
            musicOption.text = "disable music";
        } else {
            musicOption.text = "enable music";
        }
    }

    public void LoadScene(string scene) {
        SceneManager.LoadScene(scene);
    }

    public void ToggleSettings() {
        settingsScreen.SetActive(true);
        menuScreen.SetActive(false);
        ChangeMusicButtonText();
        UpdateSliderText();
        ChangeSliderValue();
    }

    public void ToggleMenu() {
        settingsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    private void ChangeSliderValue() {
        slider.value = gridSize;
    }

    public void SliderChange() {
        gridSize = (int) slider.value;
        UpdateSliderText();
        FileHandler.SaveGridsize(gridSize);
    }

    private void UpdateSliderText() {
        switch (gridSize)
            {
                case 0: 
                    sliderText.text = "Playfield size: small";
                    break;
                case 1:
                    sliderText.text = "Playfield size: medium";
                    break;
                case 2:
                    sliderText.text = "Playfield size: large";
                    break;
            }
    }


    public void ExitGame() {
        Application.Quit();
    }


}
