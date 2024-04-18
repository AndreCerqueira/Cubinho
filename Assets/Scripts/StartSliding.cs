using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSliding : MonoBehaviour
{
    private CubinhoMovement cubinhoMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        cubinhoMovement = GetComponentInParent<CubinhoMovement>();
    }

    public void StartSlidingEvent()
    {
        cubinhoMovement.StartSliding();
    }
}
