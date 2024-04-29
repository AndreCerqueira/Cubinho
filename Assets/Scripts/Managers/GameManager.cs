using System;
using System.Collections;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Sprite selectedButtonSprite, unselectedButtonSprite;
    [SerializeField] private Image levelsButton;
    
    [SerializeField] private GameObject gameOverPopUp;
    [SerializeField] private GameObject completeLevelPopUp;
    [SerializeField] private TextMeshProUGUI levelCompletedLabel;
    [SerializeField] private GameObject settingsPopUp;
    [SerializeField] private GameObject levelsPopUp;
    [SerializeField] private GameObject initialMenu;
    [SerializeField] private GameObject inGameMenu;
    private TextMeshProUGUI highScoreText;
    private TextMeshProUGUI nameText;

    private CanvasGroup levelsPopUpCanvasGroup;

    [Header("Game Over Scores")]
    [SerializeField] private TextMeshProUGUI gameOverHighScore;
    [SerializeField] private TextMeshProUGUI gameOverCurrentScore;
    [SerializeField] private TextMeshProUGUI gameOverTodayHighScore;

    [Header("Level Buttons")]
    private Color CLEARED_COLOR = new Color(206f / 255f, 255f / 255f, 206f / 255f);
    [SerializeField] private Image level1Button;
    [SerializeField] private Image level2Button;
    [SerializeField] private Image level3Button;


    void Start()
    {
        instance = this;
        levelsPopUpCanvasGroup = levelsPopUp.GetComponent<CanvasGroup>();
        highScoreText = GameObject.Find("High Score").GetComponent<TextMeshProUGUI>();
        nameText = GameObject.Find("Name").GetComponent<TextMeshProUGUI>();
        highScoreText.text = $"{Mathf.Round(PlayerPrefsManager.highScore)}m";

        Debug.Log("name as: " + PlayerPrefsManager.username);
        nameText.text = PlayerPrefsManager.username;

        var userId = AuthenticationService.Instance.PlayerId;
        GameObject.Find("UserID/Text").GetComponent<TextMeshProUGUI>().text = userId;
        
        if (PlayerPrefsManager.level1Completed == 1) level1Button.color = CLEARED_COLOR;
        if (PlayerPrefsManager.level2Completed == 1) level2Button.color = CLEARED_COLOR;
        if (PlayerPrefsManager.level3Completed == 1) level3Button.color = CLEARED_COLOR;

    }


    public void PopulateGameOverScreen(float currentScore)
    {
        gameOverCurrentScore.text = Mathf.Round(currentScore) + "m";
        gameOverTodayHighScore.text = Mathf.Round(PlayerPrefsManager.highScoreToday) + "m";
        gameOverHighScore.text = Mathf.Round(PlayerPrefsManager.highScore) + "m";
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void GoToLeaderboardScene()
    {
        SceneManager.LoadScene("Leaderboard");
    }
    
    
    public void GoToIcePassScene()
    {
        SceneManager.LoadScene("IcePass");
    }


    public void ShowGameOverPopUp()
    {
        StartCoroutine(DoFadeIn(gameOverPopUp.GetComponent<CanvasGroup>()));
    }

    
    public void ShowCompleteLevelPopUp()
    {
        levelCompletedLabel.text = PlayerPrefsManager.lastLevelLoaded.ToString();
        StartCoroutine(DoFadeIn(completeLevelPopUp.GetComponent<CanvasGroup>()));
    }


    public void ShowLevelsPopUp() 
    {
        if (IsLevelsPopUpVisible())
        {
            HideLevelsPopUp();
            return;
        }

        MarkButtonAsSelected(levelsButton);
        StartCoroutine(DoFadeIn(levelsPopUpCanvasGroup));
    }
    

    public void HideLevelsPopUp() 
    {
        MarkButtonAsUnselected(levelsButton);
        StartCoroutine(DoFadeOut(levelsPopUpCanvasGroup));
    }

    
    public void HideInitialMenu()
    {
        StartCoroutine(DoFadeOut(initialMenu.GetComponent<CanvasGroup>()));

        if (PlayerPrefsManager.lastLevelLoaded == 0)
            StartCoroutine(DoFadeIn(inGameMenu.GetComponent<CanvasGroup>()));
    }


    public void ShowSettingsMenu()
    {
        StartCoroutine(DoFadeIn(settingsPopUp.GetComponent<CanvasGroup>()));
    }

    
    public void HideSettingsMenu()
    {
        StartCoroutine(DoFadeOut(settingsPopUp.GetComponent<CanvasGroup>()));
    }


    public void MarkButtonAsSelected(Image button)
    {
        button.sprite = selectedButtonSprite;
    }


    public void MarkButtonAsUnselected(Image button)
    {
        button.sprite = unselectedButtonSprite;
    }

    
    public void LogOut()
    {
        AuthenticationService.Instance.SignOut(true);
        AuthenticationService.Instance.ClearSessionToken();
        SceneManager.LoadScene("InitialMenu");
    }


    #region Fade Effects

    static public IEnumerator DoFadeOut(CanvasGroup canvasG)
    {
        while (canvasG.alpha > 0)
        {
            canvasG.alpha -= Time.deltaTime * 2;
            yield return null;
        }

        canvasG.interactable = false;
        canvasG.blocksRaycasts = false;
    }

    static public IEnumerator DoFadeIn(CanvasGroup canvasG)
    {
        canvasG.interactable = true;
        canvasG.blocksRaycasts = true;

        while (canvasG.alpha < 1)
        {
            canvasG.alpha += Time.deltaTime * 2;
            yield return null;
        }
    }

    #endregion

    public bool IsLevelsPopUpVisible()
    {
        return levelsPopUpCanvasGroup.alpha > 0;
    }
}
