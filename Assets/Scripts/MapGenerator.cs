using System;
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
        float currentScore = ScoreCounter.instance.GetScore();

        List<int> validChunkIndices = new List<int>();
        for (int i = 0; i < chunkPrefabs.Length; i++)
        {
            if (chunkPrefabs[i].GetComponent<Chunk>().difficulty == nextChunkDifficulty)
            {
                if (!chunkPrefabs[i].GetComponent<Chunk>().isAfter600m || (chunkPrefabs[i].GetComponent<Chunk>().isAfter600m && currentScore >= 600))
                    validChunkIndices.Add(i);
            }
        }
        
        int randomIndex = UnityEngine.Random.Range(0, validChunkIndices.Count);
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


    //public void GenerateChunk()
    //{
    //    Difficulty nextChunkDifficulty = GetNextChunkDifficulty();

    //    List<int> validChunkIndices = new List<int>();
    //    for (int i = 0; i < chunkPrefabs.Length; i++)
    //    {
    //        if (chunkPrefabs[i].GetComponent<Chunk>().difficulty == nextChunkDifficulty)
    //        {
    //            validChunkIndices.Add(i);
    //        }
    //    }

    //    int randomIndex;
    //    GameObject chunk;

    //    // Se não houver índices válidos nesta lista, procuramos em outra até encontrar um índice válido
    //    while (validChunkIndices.Count == 0)
    //    {
    //        nextChunkDifficulty = (Difficulty)(((int)nextChunkDifficulty + 1) % Enum.GetNames(typeof(Difficulty)).Length);
    //        for (int i = 0; i < chunkPrefabs.Length; i++)
    //        {
    //            if (chunkPrefabs[i].GetComponent<Chunk>().difficulty == nextChunkDifficulty)
    //            {
    //                validChunkIndices.Add(i);
    //            }
    //        }
    //    }

    //    randomIndex = UnityEngine.Random.Range(0, validChunkIndices.Count);
    //    chunk = Instantiate(chunkPrefabs[validChunkIndices[randomIndex]], transform);
    //    chunk.transform.SetParent(chunkContainer);

    //    float nextChunkDistanceZ = chunk.GetComponent<Chunk>().distanceToNextChunkZ;

    //    if (lastChunkZ == 0)
    //        lastChunkZ = chunk.transform.position.z - nextChunkDistanceZ;

    //    chunk.transform.position = new Vector3(
    //        chunk.transform.position.x,
    //        chunk.transform.position.y,
    //        lastChunkZ + nextChunkDistanceZ
    //        );

    //    lastChunkZ = chunk.transform.position.z;
    //}


    public void GenerateChunk(GameObject chunkPrefab)
    {
        GameObject chunk = Instantiate(chunkPrefab, transform);
        chunk.transform.SetParent(chunkContainer);
    }
    

    private Difficulty GetNextChunkDifficulty()
    {
        float randomValue = UnityEngine.Random.value;
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
