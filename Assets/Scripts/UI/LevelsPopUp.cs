using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsPopUp : MonoBehaviour
{
    // Variables
    [SerializeField] private Button levelsButton;
    [SerializeField] private Transform deco;

    void Start()
    {
        
    }
    
    void Update()
    {
        deco.transform.position = new Vector3(levelsButton.transform.position.x, deco.transform.position.y, deco.transform.position.z);
    }
}
