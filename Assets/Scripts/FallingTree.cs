using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTree : MonoBehaviour
{
    private GameObject player;
    private Animator animator;
    private float distanceToFall = -20;
    [SerializeField] private bool isLeft;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if the player is within a certain distance, fall
        if (player.transform.position.z - transform.position.z > distanceToFall) 
        {
            if (isLeft)
                animator.SetTrigger("fallLeft");
            else
                animator.SetTrigger("fallRight");
        }
    }
}
