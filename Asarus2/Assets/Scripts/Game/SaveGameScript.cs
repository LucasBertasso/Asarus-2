using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameScript : StatsController {

    public static int
        lifeLevelReciver,
        staminaLevelReciver,
        manaLevelReciver;


	void Start () {
        // Stats load
        lifeLevel = PlayerPrefs.GetInt("lifeLevel");
        staminaLevel = PlayerPrefs.GetInt("staminaLevel");
        manaLevel = PlayerPrefs.GetInt("manaLevel");
        lifeLevelReciver = PlayerPrefs.GetInt("lifeLevel");
        staminaLevelReciver = PlayerPrefs.GetInt("staminaLevel");
        manaLevelReciver = PlayerPrefs.GetInt("manaLevel");
    }
	 void SaveGame()
    {
        // Max Stats
        PlayerPrefs.SetInt("lifeLevel", lifeLevel);
        PlayerPrefs.SetInt("staminaLevel", staminaLevel);
        PlayerPrefs.SetInt("manaLevel", manaLevel);
        // Debug
        Debug.Log("Game Saved");
    }
}
