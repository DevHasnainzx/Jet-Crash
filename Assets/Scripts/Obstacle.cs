using UnityEngine;
using System.Collections.Generic;

public class Obstacle : MonoBehaviour
{

    // Setting values for size of obstacles
    [Header("Size Attributes")]
    public float minSize = 0.5f;
    public float maxSize = 2f;

    // setting values for speed of obstacles
    [Header("Speed Attributes")]
    public float minSpeed = 50f;
    public float maxSpeed = 150f;
    public float maxVelocity = 250f;

    // adding spin variables t randomize the obstacles spin angle
    [Header("Rotation Attributes")]
    public float maxSpinSpeed = 10f;

    [Header("VFX Attributes")]
    public GameObject bounceVFX;

    // accesing Rigidbody2D compoenet to apply Force
    Rigidbody2D rb;


    void Start()
    {
        // Applying random size to the obstacles
        float randomSize = Random.Range(minSize, maxSize);

        // assessing Scale of Onstacles
        transform.localScale = new Vector3(randomSize,randomSize,1);

        // verifying component
        rb = GetComponent<Rigidbody2D>();

        // Applying random speed to the obstacles
        float randomSpeed = Random.Range(minSpeed,maxSpeed);

        //Applying random direction to the obstacles using radius of circle
        Vector2 randomDirection = Random.insideUnitCircle;

        // Applying force to move the obstacles as they have zero gravity
        rb.AddForce(randomDirection * randomSpeed);


        // adding torque to make randomized spin to the obsatcles;
        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque * randomSize);


    }

    // Update is called once per frame
    void Update()
    {
        // Clamp velocity if it exceeds maxVelocity
        if (rb.linearVelocity.magnitude > maxVelocity)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactPoint = collision.GetContact(0).point;
        GameObject bonceEffect = Instantiate(bounceVFX, contactPoint, Quaternion.identity);

        Destroy(bonceEffect,1f);
    }
}