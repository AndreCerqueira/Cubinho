using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    const int INITIAL_CHUNKS = 5;

    public static MapGenerator instance;

    private float lastChunkZ = 0;
    [SerializeField] private GameObject[] chunkPrefabs;
    [SerializeField] private Transform chunkContainer;

    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        int lastLevelLoaded = PlayerPrefsManager.lastLevelLoaded;
        LevelButton lastLevelLoadedButton = GameObject.Find("Level " + (lastLevelLoaded == 0 ? "Infinite" : lastLevelLoaded)).GetComponent<LevelButton>();
        lastLevelLoadedButton.LoadLevel();
    }
    
    
    public void GenerateChunk()
    {
        Difficulty nextChunkDifficulty = GetNextChunkDifficulty();

        List<int> validChunkIndices = new List<int>();
        for (int i = 0; i < chunkPrefabs.Length; i++)
        {
            if (chunkPrefabs[i].GetComponent<Chunk>().difficulty == nextChunkDifficulty)
            {
                validChunkIndices.Add(i);
            }
        }

        int randomIndex = Random.Range(0, validChunkIndices.Count);
        GameObject chunk = Instantiate(chunkPrefabs[validChunkIndices[randomIndex]], transform);
        chunk.transform.SetParent(chunkContainer);

        float nextChunkDistanceZ = chunk.GetComponent<Chunk>().distanceToNextChunkZ;

        if (lastChunkZ == 0)
            lastChunkZ = chunk.transform.position.z - nextChunkDistanceZ;

        chunk.transform.position = new Vector3(
            chunk.transform.position.x,
            chunk.transform.position.y,
            lastChunkZ + nextChunkDistanceZ
            );

        lastChunkZ = chunk.transform.position.z;
    }

    
    public void GenerateChunk(GameObject chunkPrefab)
    {
        GameObject chunk = Instantiate(chunkPrefab, transform);
        chunk.transform.SetParent(chunkContainer);
    }
    

    private Difficulty GetNextChunkDifficulty()
    {
        float randomValue = Random.value;
        float easySpawnChance = Difficulty.Easy.GetSpawnChance();
        float mediumSpawnChance = Difficulty.Medium.GetSpawnChance();
        float hardSpawnChance = Difficulty.Hard.GetSpawnChance();

        if (randomValue < hardSpawnChance)
        {
            return Difficulty.Hard;
        }
        else if (randomValue < mediumSpawnChance + hardSpawnChance)
        {
            return Difficulty.Medium;
        }
        else
        {
            return Difficulty.Easy;
        }
    }


    public void GenerateInitialChunks()
    {
        lastChunkZ = 0;
        for (int i = 0; i < INITIAL_CHUNKS; i++)
        {
            GenerateChunk();
        }
    }
    
    
    public void GenerateInitialChunks(GameObject chunkPrefab)
    {
        GenerateChunk(chunkPrefab);
    }
}
