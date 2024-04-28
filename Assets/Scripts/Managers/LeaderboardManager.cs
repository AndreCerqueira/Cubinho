using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
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
    [SerializeField] private GameObject medalsRow;

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

    
    public class PlayerMedals
    {
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int Points { get; set; }
        public int GoldCount { get; set; }
        public int SilverCount { get; set; }
        public int BronzeCount { get; set; }
    }


    public void Start()
    {
        currentActiveButton = allTimeHighScoreButton;
        MarkButtonAsSelected(currentActiveButton.GetComponent<Image>());

        GetAllTimeHighScores();
    }


    public void ReturnToGameScene() => SceneManager.LoadScene("Game");


    public void OpenAllTimeHighScoreLeaderboard() => OpenLeaderboard(allTimeHighScoreButton, GetAllTimeHighScores);
    public void OpenTodayLeaderboard() => OpenLeaderboard(todayButton, GetTodayHighScores);
    public void OpenYesterdayLeaderboard() => OpenLeaderboard(yesterdayButton, GetYesterdayHighScores);
    
    public void OpenLevelsLeaderboard() => OpenLeaderboard(levelsButton);
    public void OpenTopMedalistsLeaderboard() => OpenLeaderboard(topMedalistsButton, GetBestMedalists);


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

        BuildDefaultLeaderboard(scoresResponse.Results);
    }

    
    public async void GetTodayHighScores()
    {
        ClearLeaderboard();
        
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(DAY_BEST_LEADERBOARD_ID, new GetScoresOptions { Offset = 0, Limit = 10 });
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));

        BuildDefaultLeaderboard(scoresResponse.Results);
    }


    public async void GetYesterdayHighScores()
    {
        ClearLeaderboard();

        var versionsResponse = await LeaderboardsService.Instance
            .GetVersionsAsync(
                DAY_BEST_LEADERBOARD_ID,
                new GetVersionsOptions { Limit = 1 });

        var versionId = versionsResponse.Results[0].Id;

        var scoresResponse = await LeaderboardsService.Instance
        .GetVersionScoresAsync(DAY_BEST_LEADERBOARD_ID, versionId, new GetVersionScoresOptions { Offset = 0, Limit = 10 });
        
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));

        BuildDefaultLeaderboard(scoresResponse.Results);
    }


    public async void GetBestMedalists()
    {
        ClearLeaderboard();
            
        var allVersions = await LeaderboardsService.Instance.GetVersionsAsync(DAY_BEST_LEADERBOARD_ID);

        var playerScores = new List<PlayerMedals>();

        // Iterar por todas as versões do leaderboard
        foreach (var version in allVersions.Results)
        {
            var scoresResponse = await LeaderboardsService.Instance
                .GetVersionScoresAsync(DAY_BEST_LEADERBOARD_ID, version.Id, new GetVersionScoresOptions { Offset = 0, Limit = 10 });

            // Atribuir pontos com base no ranking
            for (int i = 0; i < scoresResponse.Results.Count; i++)
            {
                int points = 0;
                points = i switch
                {
                    0 => 3,
                    1 => 2,
                    2 => 1,
                    _ => 0,
                };
                var playerId = scoresResponse.Results[i].PlayerId;
                var playerName = scoresResponse.Results[i].PlayerName;
                var playerScore = playerScores.FirstOrDefault(p => p.PlayerId == playerId);

                if (playerScore != null)
                {
                    playerScore.Points += points;

                    // Atualizar a contagem de medalhas
                    switch (i)
                    {
                        case 0:
                            playerScore.GoldCount++;
                            break;
                        case 1:
                            playerScore.SilverCount++;
                            break;
                        case 2:
                            playerScore.BronzeCount++;
                            break;
                    }
                }
                else
                {
                    var newPlayerScore = new PlayerMedals
                    {
                        PlayerId = playerId,
                        PlayerName = playerName,
                        Points = points
                    };

                    // Definir a contagem de medalhas
                    switch (i)
                    {
                        case 0:
                            newPlayerScore.GoldCount = 1;
                            break;
                        case 1:
                            newPlayerScore.SilverCount = 1;
                            break;
                        case 2:
                            newPlayerScore.BronzeCount = 1;
                            break;
                    }

                    playerScores.Add(newPlayerScore);
                }
            }
        }

        // Classificar jogadores com base em seus pontos
        var sortedPlayerScores = playerScores.OrderByDescending(p => p.Points);

        // Selecionar os 10 melhores jogadores
        List<PlayerMedals> topPlayers = sortedPlayerScores.Take(10).ToList();

        BuildMedalistsLeaderboard(topPlayers);
    }


    public void ClearLeaderboard()
    {
        foreach (Transform child in leaderboardContent.transform)
        {
            Destroy(child.gameObject);
        }
    }


    public void BuildDefaultLeaderboard(List<LeaderboardEntry> results)
    {
        foreach (var result in results)
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

    public void BuildMedalistsLeaderboard(List<PlayerMedals> results)
    {
        var rank = 0;
        foreach (var result in results)
        {
            // remove the # id from the name
            string name = result.PlayerName;
            if (result.PlayerName.Contains("#"))
                name = result.PlayerName.Substring(0, result.PlayerName.LastIndexOf("#"));

            var row = Instantiate(medalsRow, leaderboardContent.transform);

            row.GetComponent<Image>().color = rank switch
            {
                0 => firstColor,
                1 => secondColor,
                2 => thirdColor,
                _ => rank % 2 == 0 ? defaultEvenColor : defaultOddColor,
            };

            Image rankIcon = row.transform.Find("Position").GetComponent<Image>();
            rankIcon.sprite = rank switch
            {
                0 => firstRank,
                1 => secondRank,
                2 => thirdRank,
                _ => defaultRank,
            };

            row.transform.Find("Position/Text").GetComponent<TextMeshProUGUI>().text = (rank + 1).ToString();
            row.transform.Find("Player name").GetComponent<TextMeshProUGUI>().text = name;

            GameObject goldCounter = row.transform.Find("Score/Content/Golds").gameObject;
            if (result.GoldCount > 0)
                goldCounter.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "x" + result.GoldCount;
            else
                goldCounter.SetActive(false);

            GameObject silverCounter = row.transform.Find("Score/Content/Silvers").gameObject;
            if (result.SilverCount > 0)
                silverCounter.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "x" + result.SilverCount;
            else
                silverCounter.SetActive(false);

            GameObject bronzeCounter = row.transform.Find("Score/Content/Bronzes").gameObject;
            if (result.BronzeCount > 0)
                bronzeCounter.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "x" + result.BronzeCount;
            else
                bronzeCounter.SetActive(false);

            rank++;
        }
    }
}
