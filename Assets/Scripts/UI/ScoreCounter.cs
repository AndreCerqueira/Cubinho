using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter instance;

    private TextMeshProUGUI scoreText;
    [SerializeField] private Transform player;

    private float initialDistance;
    private float currentDistance => player.position.z;
    private float nextSpeedIncrement = 100;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        scoreText = GetComponent<TextMeshProUGUI>();
        initialDistance = player.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"{Mathf.Round(GetScore())}m";
    }
    
    
    public float GetScore()
    {
        var score = currentDistance - initialDistance;
        if (score > nextSpeedIncrement)
        {
            nextSpeedIncrement += 100;
            player.GetComponent<CubinhoMovement>().IncrementSpeed();
        }
        
        return currentDistance - initialDistance;
    }
}
