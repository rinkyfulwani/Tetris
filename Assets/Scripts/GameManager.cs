using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
   public static GameManager currentInstance;

   private float currentGameSpeed;
   private int currentScore = 0;
   private int clearedLayers = 0;
   private int currentLevel = 1;
   private float highspeed = 0.05f;
   private bool isHighSpeed = false;
   private bool isGameOver = false;
   private bool isMusicEnabled = true;

   private int loadedHiscore;

   private int gridSize = 3;

   private int hiscore;

   private bool isGamePaused;

   public AudioSource themeMusic;

   private int[] scoreThresholds = {5000, 10000, 15000, 20000, 25000, 30000, 35000};
   private int[] comboScores = {2000, 5000, 10000, 15000};
   private float[] gameSpeeds = {1.5f, 1.2f, 1.0f, 0.8f, 0.6f, 0.4f};

   private int[][] gameSizes = { new int[] { 4, 8, 4 }, new int[] { 6, 10, 6 }, new int[] { 7, 12, 7 } };
    
   private void Start() {
       LoadGameData();
       Time.timeScale = 1;
       Grid.Instance.ChangeGridSize(gameSizes[gridSize][0], gameSizes[gridSize][1], gameSizes[gridSize][2]);
       currentInstance = this;
       currentGameSpeed = gameSpeeds[0];
       UIController.currentInstance.SetUIText(hiscore, currentScore, currentLevel, clearedLayers);
       UIController.currentInstance.SetToggleMusicText(isMusicEnabled);
       UIController.currentInstance.ShowGameOverScreen(false);
       UIController.currentInstance.ShowPauseScreen(false);
       if (!isMusicEnabled) {
           themeMusic.Stop();
       }
   }

   public bool GetIsGamePaused() {
       return isGamePaused;
   }
   
   private void LoadGameData() {
       GameData gameData = FileHandler.LoadGameData();
       hiscore = gameData.hiscore;
       loadedHiscore = hiscore;
       isMusicEnabled = gameData.isMusicEnabled;
       gridSize = gameData.gridSizeSetting;
   }
   
    public void UpdateScore(int incrementScore) {
        currentScore = currentScore + incrementScore;
        ComputeCurrentLevel();
        if (currentScore > hiscore) {
            hiscore = currentScore;
            FileHandler.SaveHiscore(hiscore);
        }
        UIController.currentInstance.SetUIText(hiscore, currentScore, currentLevel, clearedLayers);
    }

    // increment the cleared layers and update the score depending on how many rows are cleared
    public void UpdateClearedLayers(int incrementLayers) {
        clearedLayers = clearedLayers + incrementLayers;
        UpdateScore(comboScores[incrementLayers - 1]);
    }

    // logic for computing the current layer based on the current score
    private void ComputeCurrentLevel() {
        for (int i = 0; i < scoreThresholds.Length - 1; i++)
        {
            if (currentScore < scoreThresholds[i]) {
                currentLevel = Array.IndexOf(scoreThresholds, scoreThresholds[i]) + 1;
                if (!isHighSpeed) {
                    currentGameSpeed = gameSpeeds[i];
                }
                break;
            }
        }
    }

    public float GetGameSpeed() {
        return currentGameSpeed;
    }

    public void SetBlockSpeed(float speed)
    {
        currentGameSpeed = speed;
    }

    public void SetHighSpeed() {
        currentGameSpeed = highspeed;
        isHighSpeed = true;
        UIController.currentInstance.ToggleSpeedButton();
    }
    
    public void SetNormalSpeed() {
        currentGameSpeed = gameSpeeds[currentLevel];
        isHighSpeed = false;
        UIController.currentInstance.UntoggleSpeedButton();
    }

    public void SetIsGameOver(bool val) {
        isGameOver = val;
        bool isNewHiscore = false;
        UIController.currentInstance.ShowGameOverScreen(val);
        if (currentScore > loadedHiscore) {
            isNewHiscore = true;
        }
        UIController.currentInstance.SetFinalScoreText(isNewHiscore, currentScore);
    }

    public bool GetIsGameOver() {
        return isGameOver;
    }


    public void PauseGame() {
        UIController.currentInstance.ShowPauseScreen(true);
        themeMusic.Pause();
        Time.timeScale = 0;
        isGamePaused = true;
    }

    public void ResumeGame() {
        UIController.currentInstance.ShowPauseScreen(false);
        if (isMusicEnabled) {
            themeMusic.Play();
        }
        Time.timeScale = 1;
        isGamePaused = false;
    }

    public void ToggleMusic() {
        if (isMusicEnabled) {
            themeMusic.Stop();
            UIController.currentInstance.SetToggleMusicText(false);
        } else {
            UIController.currentInstance.SetToggleMusicText(true);
        }
        isMusicEnabled = !isMusicEnabled;
        FileHandler.SaveMusicSetting(isMusicEnabled);

    }

    public void RestartGame() {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMenu() {
        SceneManager.LoadScene("MenuScene");
    }

}
