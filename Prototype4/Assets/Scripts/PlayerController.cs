﻿/* 
 * Zach Wilson
 * Assignment 7
 * This script manages the movement of the player object
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //settings variables
    public float speed = 5.0f;
    private float powerupStrength = 15.0f;

    //object variables
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject powerupIndicator;

    //changing variables
    private float forwardInput;
    public bool hasPowerup;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.FindGameObjectWithTag("FocalPoint");
    }

    // Update is called once per frame
    void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);
    }

    private void FixedUpdate()
    {
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(hasPowerup);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(hasPowerup);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Debug.Log("Player collided with " + collision.gameObject.name + " with powerup set to " + hasPowerup);

            //get a local reference to enemy rigidbody
            Rigidbody enemyRigidBody = collision.gameObject.GetComponent<Rigidbody>();

            //set a vector3 with a direction away from the player
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position).normalized;

            //add force away from player on enemy
            enemyRigidBody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }
}
