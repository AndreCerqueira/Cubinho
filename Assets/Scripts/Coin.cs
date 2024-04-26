using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float bounceHeight;
    [SerializeField] private float bounceSpeed;
    private Vector3 initialPosition;
    private Transform model;

    private void Start()
    {
        initialPosition = transform.position;
        model = GetComponentInChildren<MeshRenderer>().transform;
        player = GameObject.Find("Player").transform;
    }

    
    void Update()
    {
        model.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        float bounceOffset = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        model.position = initialPosition + new Vector3(0f, bounceOffset, 0f);

        // if player is too far away from the coin, destroy it
        if (transform.position.z - player.position.z < -5)
            Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Coin collected");
            PlayerPrefsManager.runCoins++;
            Destroy(gameObject);
        }
    }
}
