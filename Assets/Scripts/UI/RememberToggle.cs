using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RememberToggle : MonoBehaviour
{
    [SerializeField] private GameObject check;

    void Start()
    {
        AuthManager.rememberMe = check.activeSelf;
    }


    public void Click()
    {
        check.SetActive(!check.activeSelf);
        AuthManager.rememberMe = check.activeSelf;
    }
}
