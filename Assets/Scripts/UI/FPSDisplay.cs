using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private TextMeshProUGUI fpsText;

    void Start()
    {
        fpsText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float fps = 1f / Time.deltaTime;
        fpsText.text = "V6.0 FPS: " + Mathf.Round(fps);
    }
}
