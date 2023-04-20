using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIController : MonoBehaviour
{
    public static UIController currentInstance;

    public TMP_Text hiscore;
    public TMP_Text currentScore;
    public TMP_Text currentLevel;
    public TMP_Text clearedLayers;
    public GameObject gameOverScreen;
    public GameObject pausedScreen;

    public TMP_Text toggleMusicText;

    public TMP_Text finalScoreText;

    public GameObject speedButton;

    private void Awake() {
        currentInstance = this;
    }

    public void SetUIText(int hi, int updateScore, int updateLevel, int updateClearedLayers) {
        hiscore.text = "Hiscore: " + hi.ToString();
        currentScore.text = "Score: " + updateScore.ToString();
        currentLevel.text = "Level: " + updateLevel.ToString();
        clearedLayers.text = "Layers: " + updateClearedLayers.ToString();
    }

    public void SetFinalScoreText(bool newHiscore, int finalScore) {
        if (newHiscore) {
            finalScoreText.text = "You got new hiscore: " + finalScore.ToString();
        } else {
            finalScoreText.text = "Your score was: " + finalScore.ToString();
        }
    }

    public void ShowGameOverScreen(bool val) {
        gameOverScreen.SetActive(val);
    }

    public void ShowPauseScreen(bool val) {
        pausedScreen.SetActive(val);
    }

    public void SetToggleMusicText(bool isMusicOn) {
        if (isMusicOn) {
            toggleMusicText.text = "disable music";
        } else {
            toggleMusicText.text = "enable music";
        }
    }

    public void ToggleSpeedButton() {
        speedButton.GetComponent<Image>().color = Color.red;
    }

    public void UntoggleSpeedButton() {
        speedButton.GetComponent<Image>().color = Color.white;
    }
}
