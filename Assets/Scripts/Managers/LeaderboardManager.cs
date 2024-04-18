using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    // Variables
    private GameObject currentActiveLeaderboard;
    private Button currentActiveButton;

    [Header("Sprites")]
    [SerializeField] private Sprite selectedButtonSprite;
    [SerializeField] private Sprite unselectedButtonSprite;

    [Header("Leaderboards")]
    [SerializeField] private GameObject allTimeHighScoreLeaderboard;
    [SerializeField] private GameObject todayLeaderboard;
    [SerializeField] private GameObject yesterdayLeaderboard;
    [SerializeField] private GameObject levelsLeaderboard;
    [SerializeField] private GameObject topMedalistsLeaderboard;

    [Header("Buttons")]
    [SerializeField] private Button allTimeHighScoreButton;
    [SerializeField] private Button todayButton;
    [SerializeField] private Button yesterdayButton;
    [SerializeField] private Button levelsButton;
    [SerializeField] private Button topMedalistsButton;


    public void Start()
    {
        currentActiveLeaderboard = allTimeHighScoreLeaderboard;
        currentActiveButton = allTimeHighScoreButton;
        MarkButtonAsSelected(currentActiveButton.GetComponent<Image>());
    }

    
    public void ReturnToGameScene() => SceneManager.LoadScene(0);


    public void OpenAllTimeHighScoreLeaderboard() => OpenLeaderboard(allTimeHighScoreLeaderboard, allTimeHighScoreButton);
    public void OpenTodayLeaderboard() => OpenLeaderboard(todayLeaderboard, todayButton);
    public void OpenYesterdayLeaderboard() => OpenLeaderboard(yesterdayLeaderboard, yesterdayButton);
    public void OpenLevelsLeaderboard() => OpenLeaderboard(levelsLeaderboard, levelsButton);
    public void OpenTopMedalistsLeaderboard() => OpenLeaderboard(topMedalistsLeaderboard, topMedalistsButton);
    

    public void OpenLeaderboard(GameObject leaderboard, Button button)
    {
        currentActiveLeaderboard.SetActive(false);
        currentActiveLeaderboard = leaderboard;
        currentActiveLeaderboard.SetActive(true);

        MarkButtonAsUnselected(currentActiveButton.GetComponent<Image>());
        currentActiveButton = button;
        MarkButtonAsSelected(currentActiveButton.GetComponent<Image>());
    }

    public void MarkButtonAsSelected(Image button) => button.sprite = selectedButtonSprite;
    public void MarkButtonAsUnselected(Image button) => button.sprite = unselectedButtonSprite;
}
