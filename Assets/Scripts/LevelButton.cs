using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    // Constants
    const float INITIAL_INFINITE_LEVEL_SLIDING_SPEED = 20f;
    const float INITIAL_INFINITE_LEVEL_TURNING_SPEED = 15f;

    // Variables
    public int levelIndex;
    public bool isCompleted;
    public bool isInfinite;
    public float levelSlidingSpeed;
    public float levelTurningSpeed;
    public GameObject chunkPrefab;
    private TextMeshProUGUI levelText;
    private CubinhoMovement player;

    // Start is called before the first frame update
    void Awake()
    {
        levelText = GameObject.Find("Level Text").GetComponent<TextMeshProUGUI>();
        player = GameObject.Find("Player").GetComponent<CubinhoMovement>();

        GetComponent<Button>().onClick.AddListener(LoadLevel);

        if (isInfinite) return;

        // if completed change image color to #BDFFC2
        GetComponent<Image>().color = isCompleted ? new Color(189f / 255f, 1f, 194f / 255f) : Color.white;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // on click
    public void LoadLevel()
    {

        Debug.Log("LoadLevel ");

        levelText.text = isInfinite ? $"Infinite" : $"Level {levelIndex}";
        PlayerPrefsManager.lastLevelLoaded = isInfinite ? 0 : levelIndex;
        player.slidingSpeed = isInfinite ? INITIAL_INFINITE_LEVEL_SLIDING_SPEED : levelSlidingSpeed;
        player.turningSpeed = isInfinite ? INITIAL_INFINITE_LEVEL_TURNING_SPEED : levelTurningSpeed;

        // Remove all chunks from the scene
        var chunks = GameObject.FindGameObjectsWithTag("Chunk");
        foreach (var chunk in chunks)
            Destroy(chunk);

        // Generate initial chunks
        if (isInfinite)
            MapGenerator.instance.GenerateInitialChunks();
        else
            MapGenerator.instance.GenerateInitialChunks(chunkPrefab);
    }
}
