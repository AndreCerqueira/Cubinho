using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSliding : MonoBehaviour
{
    private CubinhoMovement cubinhoMovement;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        cubinhoMovement = GetComponentInParent<CubinhoMovement>();
        // audioSource = FindAnyObjectByType<AudioSource>();
    }

    public void StartSlidingEvent()
    {
        cubinhoMovement.StartSliding();
        // audioSource.Play();
    }
}
