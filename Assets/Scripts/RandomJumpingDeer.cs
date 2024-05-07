using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomJumpingDeer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Deer>().isJumpingDeer = Random.Range(0, 4) != 0;
    }
}
