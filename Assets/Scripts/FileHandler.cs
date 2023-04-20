using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileHandler : MonoBehaviour
{
    public static void SaveGameData(GameData gameData) {
	    PlayerPrefs.SetInt("hiscore", gameData.hiscore);
	    PlayerPrefs.SetInt("isMusicEnabled", gameData.isMusicEnabled?1:0);
	    PlayerPrefs.SetInt("gridSizeSetting", gameData.gridSizeSetting);
	    PlayerPrefs.Save();
	    Debug.Log("Saved data!");
    }

    public static GameData LoadGameData() {
        GameData gameData;

	    if (PlayerPrefs.HasKey("isMusicEnabled"))
	    {
		    int hiscore = PlayerPrefs.GetInt("hiscore");
		    bool isMusicEnabled = PlayerPrefs.GetInt("isMusicEnabled")==1?true:false;
		    int gridSizeSetting = PlayerPrefs.GetInt("gridSizeSetting");
            gameData = new GameData(hiscore, isMusicEnabled, gridSizeSetting);
		    Debug.Log("Loaded game data");

	    } else {
		    Debug.Log("Could not find any saved game data!");
            gameData = new GameData(0, true, 1);
            }
        return gameData;
    }


    public static void SaveHiscore(int hiscore) {
        PlayerPrefs.SetInt("hiscore", hiscore);
        Debug.Log("Saved hiscore setting!");
    }
    public static void SaveMusicSetting(bool isMusicEnabled) {
        PlayerPrefs.SetInt("isMusicEnabled", isMusicEnabled?1:0);
        Debug.Log("Saved music setting!");
    }

    public static void SaveGridsize(int gridsize) {
        PlayerPrefs.SetInt("gridSizeSetting", gridsize);
        Debug.Log("Saved gridsize setting!");
    }

    public static void ResetData() {
	    PlayerPrefs.DeleteAll();
	    Debug.Log("Game data reset is complete!");
    }
}
