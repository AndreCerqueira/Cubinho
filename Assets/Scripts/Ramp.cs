using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }

    // on collision enter
    private void OnCollisionEnter(Collision collision)
    {
        // if the player is colliding with the ramp, set the gravity to false
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player is colliding with the ramp");
        }
    }

    // on collision exit
    private void OnCollisionExit(Collision collision)
    {
        // if the player is leaving the ramp, reset the gravity
        if (collision.gameObject.tag == "Player")
        {
            // add a force to the player
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
        }
    }
}
