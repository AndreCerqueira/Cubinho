using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private float chanceToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        // check if we should spawn a coin
        if (Random.value < chanceToSpawn)
        {
            // spawn a coin
            GameObject coin = Instantiate(Resources.Load("Coin"), transform.position, Quaternion.identity) as GameObject;
            coin.transform.parent = transform;
        }
        else
            Destroy(gameObject);
    }

}
