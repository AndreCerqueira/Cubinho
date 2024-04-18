using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // randomize rotation in z
        transform.Rotate(0, 0, Random.Range(-7.5f, 7.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
