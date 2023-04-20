using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
     public int hiscore;
     public bool isMusicEnabled;
     public int gridSizeSetting;
 
     public GameData(int score, bool music, int grid)
     {
         hiscore = score;
         isMusicEnabled = music;
         gridSizeSetting = grid;
     }

}
