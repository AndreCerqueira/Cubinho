using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Services.CloudSave;

public class AuthManager : MonoBehaviour
{
    public static bool rememberMe;
    
    [SerializeField] private CanvasGroup toast;
    [SerializeField] private TextMeshProUGUI toastMessage;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;

    string username => usernameInput.text;
    string password => passwordInput.text;

    async void Start()
    {
        await UnityServices.InitializeAsync();
        print("Unity Services Initialized");

        AuthenticationService.Instance.ClearSessionToken();

        if (PlayerPrefsManager.rememberMe && AuthenticationService.Instance.SessionTokenExists)
        {
            await SignInWithUsernamePasswordAsync(PlayerPrefsManager.username, PlayerPrefsManager.password);
        }
        else
        {
            AuthenticationService.Instance.ClearSessionToken();
            usernameInput.text = PlayerPrefsManager.username;
            passwordInput.text = PlayerPrefsManager.password;
        }
    }


    public async void SignInGuest()
    {
        await SignInAnonymously();
    }

    
    public async void SignInWithUsernamePassword()
    {
        await SignInWithUsernamePasswordAsync(username, password);
    }

    
    public async void SignUpWithUsernamePassword()
    {
        await SignUpWithUsernamePasswordAsync(username, password);
    }


    async Task SignInAnonymously()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            print($"Sign in Successful. [Player ID: {AuthenticationService.Instance.PlayerId}]");
            GoToGameScene(true);
        }
        catch (AuthenticationException e)
        {
            print("Sign in Failed \n" + e.Message);
        }
    }


    async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
            Debug.Log($"SignUp is successful. [Player ID: {AuthenticationService.Instance.PlayerId}]\"");
            
            GoToGameScene();
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
            ShowToast(ex.Message);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            ShowToast(ex.Message);
        }
    }


    async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log($"SignIn is successful. [Player ID: {AuthenticationService.Instance.PlayerId}]\"");

            // load coins, equiped hat and best score and today best score
            LoadCloudData();
            
            GoToGameScene();
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
            ShowToast(ex.Message);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            ShowToast(ex.Message);
        }
    }

    private async void ShowToast(string message)
    {
        toastMessage.text = message;
        StartCoroutine(GameManager.DoFadeIn(toast));

        int delay = 1000 + message.Length * 50;
        await Task.Delay(delay);

        StartCoroutine(GameManager.DoFadeOut(toast));
        toastMessage.text = "";
    }

    // change scene to game scene
    private async void GoToGameScene(bool isGuest = false)
    {
        if (isGuest)
            PlayerPrefsManager.username = await AuthenticationService.Instance.GetPlayerNameAsync(); //"guest-" + AuthenticationService.Instance.PlayerId;
        else
            PlayerPrefsManager.username = username;

        PlayerPrefsManager.password = password;
        PlayerPrefsManager.rememberMe = rememberMe;
        PlayerPrefsManager.isGuest = isGuest;

        Debug.Log("1 name as: " + username);
        SceneManager.LoadScene("Game");
    }


    public async void LoadCloudData()
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> {
          "coins", "hatEquipedId"
        });

        if (playerData.TryGetValue("coins", out var firstKey))
        {
            PlayerPrefsManager.coins = firstKey.Value.GetAs<int>();
        }
        else
        {
            PlayerPrefsManager.coins = 0;
        }

        if (playerData.TryGetValue("hatEquipedId", out var secondKey))
        {
            PlayerPrefsManager.hatEquipedId = secondKey.Value.GetAs<int>();
        }
        else
        {
            PlayerPrefsManager.hatEquipedId = 0;
        }
    }
}
