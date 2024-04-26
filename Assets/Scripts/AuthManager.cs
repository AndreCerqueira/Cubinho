using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;

public class AuthManager : MonoBehaviour
{
    
    async void Start()
    {
        await UnityServices.InitializeAsync();
        print("Unity Services Initialized");
    }

    
    public async void SignInGuest()
    {
        await SignInAnonymously();
    }

    
    async Task SignInAnonymously()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            print($"Sign in Successful. [Player ID: {AuthenticationService.Instance.PlayerId}]");
        }
        catch (AuthenticationException e)
        {
            print("Sign in Failed \n" + e.Message);
        }
    }
}
