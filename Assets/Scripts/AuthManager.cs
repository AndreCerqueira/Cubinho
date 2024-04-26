using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

        // AuthenticationService.Instance.ClearSessionToken();

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
            Debug.Log("SignUp is successful.");
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
            Debug.Log("SignIn is successful.");
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
    private void GoToGameScene(bool isGuest = false)
    {
        if (isGuest)
            PlayerPrefsManager.username = "guest-" + AuthenticationService.Instance.PlayerId;
        else
            PlayerPrefsManager.username = username;

        PlayerPrefsManager.password = password;
        PlayerPrefsManager.rememberMe = rememberMe;
        PlayerPrefsManager.isGuest = isGuest;

        Debug.Log("1 name as: " + username);
        SceneManager.LoadScene("Game");
    }

}
