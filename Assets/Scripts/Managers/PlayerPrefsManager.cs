using System;
using UnityEngine;

public static class PlayerPrefsManager
{
    public const int COINS_PER_LEVEL = 10;

    public static string getTodayDate => DateTime.Now.ToString("dd-MM-yyyy");

    public static string username
    {
        get { return PlayerPrefs.GetString("username"); }
        set { PlayerPrefs.SetString("username", value); }
    }
    
    public static string password
    {
        get { return PlayerPrefs.GetString("password"); }
        set { PlayerPrefs.SetString("password", value); }
    }
    
    public static int qualitySettings
    {
        get { return PlayerPrefs.GetInt("qualitySettings"); }
        set { PlayerPrefs.SetInt("qualitySettings", value); }
    }

    public static bool isGuest
    {
        get { return PlayerPrefs.GetInt("isGuest") == 0 ? false : true; }
        set { PlayerPrefs.SetInt("isGuest", (value == false) ? 0 : 1); }
    }

    public static bool rememberMe
    {
        get { return PlayerPrefs.GetInt("rememberMe") == 0 ? false : true; }
        set { PlayerPrefs.SetInt("rememberMe", (value == false) ? 0 : 1); }
    }

    public static int lastLevelLoaded
    {
        get { return PlayerPrefs.GetInt("lastLevelLoaded"); }
        set { PlayerPrefs.SetInt("lastLevelLoaded", value); }
    }
    
    public static float highScore
    {
        get { return PlayerPrefs.GetFloat("highScore"); }
        set { PlayerPrefs.SetFloat("highScore", value); }
    }

    public static float highScoreToday
    {
        get { return PlayerPrefs.GetFloat($"highScore-{getTodayDate}"); }
        set { PlayerPrefs.SetFloat($"highScore-{getTodayDate}", value); }
    }

    public static int hatEquipedId
    {
        get { return PlayerPrefs.GetInt("hatEquipedId"); }
        set { PlayerPrefs.SetInt("hatEquipedId", value); }
    }
    
    public static int level1Completed
    {
        get { return PlayerPrefs.GetInt("level-1"); }
        set { PlayerPrefs.SetInt("level-1", value); }
    }

    public static int level2Completed
    {
        get { return PlayerPrefs.GetInt("level-2"); }
        set { PlayerPrefs.SetInt("level-2", value); }
    }
    
    public static int level3Completed
    {
        get { return PlayerPrefs.GetInt("level-3"); }
        set { PlayerPrefs.SetInt("level-3", value); }
    }

    public static int coins
    {
        get { return PlayerPrefs.GetInt("coins"); }
        set 
        { 
            PlayerPrefs.SetInt("coins", value);
            onCoinAdded?.Invoke(null, EventArgs.Empty);
        }
    }

    public static int runCoins
    {
        get { return PlayerPrefs.GetInt("runCoins"); }
        set
        {
            PlayerPrefs.SetInt("runCoins", value);
            onCoinAdded?.Invoke(null, EventArgs.Empty);
        }
    }

    public static int currentLevel => Mathf.FloorToInt(coins / COINS_PER_LEVEL) + 1;


    public static event EventHandler<EventArgs> onCoinAdded;
}
