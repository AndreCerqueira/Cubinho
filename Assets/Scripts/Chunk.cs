using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Difficulty difficulty;
    public float distanceToNextChunkZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // on collision exit with player
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collision Exit 2");
            StartCoroutine(DestroyChunk());
            MapGenerator.instance.GenerateChunk();
        }
    }
    

    // corroutine to destroy the chunk after a certain amount of time
    private IEnumerator DestroyChunk()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
