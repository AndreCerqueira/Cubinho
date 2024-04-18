using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    // Variables
    [SerializeField] private TextMeshProUGUI progressLabel;
    [SerializeField] private TextMeshProUGUI levelLabel;
    [SerializeField] private Slider slider;


    // start
    void Start()
    {
        slider.maxValue = PlayerPrefsManager.COINS_PER_LEVEL;

        PlayerPrefsManager.onCoinAdded += UpdateLevelLabel;
        PlayerPrefsManager.onCoinAdded += UpdateProgressLabel;
        PlayerPrefsManager.onCoinAdded += UpdateSlider;

        UpdateLevelLabel(this, EventArgs.Empty);
        UpdateProgressLabel(this, EventArgs.Empty);
        UpdateSlider(this, EventArgs.Empty);
    }


    public void UpdateLevelLabel(object sender, EventArgs args)
    {
        levelLabel.text = PlayerPrefsManager.currentLevel.ToString();
    }


    public void UpdateProgressLabel(object sender, EventArgs args)
    {
        progressLabel.text = $"{GetProgress()}/{PlayerPrefsManager.COINS_PER_LEVEL}";
    }


    public void UpdateSlider(object sender, EventArgs args)
    {
        slider.value = GetProgress();
    }


    private float GetProgress() => PlayerPrefsManager.coins % PlayerPrefsManager.COINS_PER_LEVEL;
}
