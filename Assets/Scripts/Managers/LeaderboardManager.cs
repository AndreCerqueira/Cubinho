using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public static string ALL_TIME_HIGH_LEADERBOARD_ID = "1";
    public static string DAY_BEST_LEADERBOARD_ID = "2";

    // Variables
    private Button currentActiveButton;

    [Header("Rank Icon")]
    [SerializeField] private Sprite firstRank;
    [SerializeField] private Sprite secondRank;
    [SerializeField] private Sprite thirdRank;
    [SerializeField] private Sprite defaultRank;

    [Header("Colors")]
    [SerializeField] private Color firstColor;
    [SerializeField] private Color secondColor;
    [SerializeField] private Color thirdColor;
    [SerializeField] private Color defaultEvenColor;
    [SerializeField] private Color defaultOddColor;

    [Header("Rows")]
    [SerializeField] private GameObject metersRow;

    [Header("Sprites")]
    [SerializeField] private Sprite selectedButtonSprite;
    [SerializeField] private Sprite unselectedButtonSprite;

    [Header("Leaderboards")]
    [SerializeField] private GameObject leaderboard;
    [SerializeField] private GameObject leaderboardContent;

    [Header("Buttons")]
    [SerializeField] private Button allTimeHighScoreButton;
    [SerializeField] private Button todayButton;
    [SerializeField] private Button yesterdayButton;
    [SerializeField] private Button levelsButton;
    [SerializeField] private Button topMedalistsButton;


    public void Start()
    {
        currentActiveButton = allTimeHighScoreButton;
        MarkButtonAsSelected(currentActiveButton.GetComponent<Image>());

        GetAllTimeHighScores();
    }


    public void ReturnToGameScene() => SceneManager.LoadScene("Game");


    public void OpenAllTimeHighScoreLeaderboard() => OpenLeaderboard(allTimeHighScoreButton, GetAllTimeHighScores);
    public void OpenTodayLeaderboard() => OpenLeaderboard(todayButton, GetTodayHighScores);

    public void OpenYesterdayLeaderboard() => OpenLeaderboard(yesterdayButton);
    public void OpenLevelsLeaderboard() => OpenLeaderboard(levelsButton);
    public void OpenTopMedalistsLeaderboard() => OpenLeaderboard(topMedalistsButton);


    public void OpenLeaderboard(Button button, Action onLeaderboardOpened = null)
    {
        MarkButtonAsUnselected(currentActiveButton.GetComponent<Image>());
        currentActiveButton = button;
        MarkButtonAsSelected(currentActiveButton.GetComponent<Image>());

        onLeaderboardOpened?.Invoke();
    }

    public void MarkButtonAsSelected(Image button) => button.sprite = selectedButtonSprite;
    public void MarkButtonAsUnselected(Image button) => button.sprite = unselectedButtonSprite;

    
    public async void GetAllTimeHighScores()
    {
        ClearLeaderboard();

        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(ALL_TIME_HIGH_LEADERBOARD_ID, new GetScoresOptions { Offset = 0, Limit = 10 });
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        
        // instantiate a row for each score
        foreach (var result in scoresResponse.Results)
        {
            // remove the # id from the name
            string name = result.PlayerName;
            if (result.PlayerName.Contains("#"))
                name = result.PlayerName.Substring(0, result.PlayerName.LastIndexOf("#"));

            var row = Instantiate(metersRow, leaderboardContent.transform);

            row.GetComponent<Image>().color = result.Rank switch
            {
                0 => firstColor,
                1 => secondColor,
                2 => thirdColor,
                _ => result.Rank % 2 == 0 ? defaultEvenColor : defaultOddColor,
            };

            Image rankIcon = row.transform.Find("Position").GetComponent<Image>();
            rankIcon.sprite = result.Rank switch
            {
                0 => firstRank,
                1 => secondRank,
                2 => thirdRank,
                _ => defaultRank,
            };
            
            row.transform.Find("Position/Text").GetComponent<TextMeshProUGUI>().text = (result.Rank + 1).ToString();
            row.transform.Find("Player name").GetComponent<TextMeshProUGUI>().text = name;
            row.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt((float)result.Score).ToString() + "m";
        }
    }

    public async void GetTodayHighScores()
    {
        ClearLeaderboard();
        
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(DAY_BEST_LEADERBOARD_ID, new GetScoresOptions { Offset = 0, Limit = 10 });
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));

        // instantiate a row for each score
        foreach (var result in scoresResponse.Results)
        {
            // remove the # id from the name
            string name = result.PlayerName;
            if (result.PlayerName.Contains("#"))
                name = result.PlayerName.Substring(0, result.PlayerName.LastIndexOf("#"));

            var row = Instantiate(metersRow, leaderboardContent.transform);

            row.GetComponent<Image>().color = result.Rank switch
            {
                0 => firstColor,
                1 => secondColor,
                2 => thirdColor,
                _ => result.Rank % 2 == 0 ? defaultEvenColor : defaultOddColor,
            };

            Image rankIcon = row.transform.Find("Position").GetComponent<Image>();
            rankIcon.sprite = result.Rank switch
            {
                0 => firstRank,
                1 => secondRank,
                2 => thirdRank,
                _ => defaultRank,
            };

            row.transform.Find("Position/Text").GetComponent<TextMeshProUGUI>().text = (result.Rank + 1).ToString();
            row.transform.Find("Player name").GetComponent<TextMeshProUGUI>().text = name;
            row.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt((float)result.Score).ToString() + "m";
        }
    }

    
    public void ClearLeaderboard()
    {
        foreach (Transform child in leaderboardContent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
