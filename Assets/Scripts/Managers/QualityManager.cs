using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualityManager : MonoBehaviour
{
    [SerializeField] private Image lowCheck, mediumCheck, highCheck;

    void Awake()
    {
        SetQuality(PlayerPrefsManager.qualitySettings == 0 ? 2 : PlayerPrefsManager.qualitySettings);
    }

    public void SetQuality(int id)
    {
        QualitySettings.SetQualityLevel(id);
        SetQualityCheck(id);
        PlayerPrefsManager.qualitySettings = id;
    }

    public void SetQualityCheck(int id)
    {
        lowCheck.enabled = false;
        mediumCheck.enabled = false;
        highCheck.enabled = false;

        switch (id)
        {
            case 1:
                lowCheck.enabled = true;
                break;
            case 2:
                mediumCheck.enabled = true;
                break;
            case 3:
                highCheck.enabled = true;
                break;
        }
    }
}
