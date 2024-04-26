using System;
using UnityEngine;

public static class PlayerPrefsManager
{
    public const int COINS_PER_LEVEL = 10;

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

    public static int hatEquipedId
    {
        get { return PlayerPrefs.GetInt("hatEquipedId"); }
        set { PlayerPrefs.SetInt("hatEquipedId", value); }
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

    public static int currentLevel => Mathf.FloorToInt(coins / COINS_PER_LEVEL) + 1;


    public static event EventHandler<EventArgs> onCoinAdded;
}
