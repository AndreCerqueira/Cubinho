using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meta : MonoBehaviour
{

    // collision with player
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // call the method to finish the game
            collision.gameObject.GetComponent<CubinhoMovement>().CompleteLevel();
        }
    }

}
