using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deer : MonoBehaviour
{
    private GameObject player;
    private Animator animator;
    [SerializeField] private float distanceToJump = -60;
    [SerializeField] private bool jumpToLeft;
    [SerializeField] private float forceUp;
    [SerializeField] private float forceLeft;
    public bool isJumpingDeer;
    private bool jumped = false;

    
    void Start()
    {
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (player.transform.position.z - transform.position.z > distanceToJump && isJumpingDeer && !jumped) { 
            animator.SetTrigger("alert");
            jumped = true;
        }
    }

    
    public void Jump()
    {
        if (!isJumpingDeer)
            return;

        this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * forceUp, ForceMode.Impulse);

        // check rotation on y to see if it is facing left or right
        if (jumpToLeft)
            this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * forceLeft, ForceMode.Impulse);
        else
            this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * forceLeft, ForceMode.Impulse);
    }
    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Debug.Log("Enter");
            GetComponent<Animator>().SetBool("isGrounded", true);
        }
    }

    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            GetComponent<Animator>().SetBool("isGrounded", false);
        }
    }
}
