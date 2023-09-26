using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Base Stats for Movements")]
    public Rigidbody rb;
    public Vector3 pushForce;
    public float power;
    public float fallOffTimer;

    [Header("Positional Checks / Targets")] //--this is used to have the player utilize some form of attached objects or attached "PID FOLLOWER" like a companion (NavMeshAgent Companion will be tested in the AutoFollower)
    public Transform targetPosition;
    public Transform attachTarget;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //forward or update movement will be the implementation for W
        if(Input.GetKey(KeyCode.W))
        {
            //move forward or upward
            rb.AddForce(transform.forward * power, ForceMode.Impulse);
        }
        //backwards or down movement will be the implementation for W
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * power, ForceMode.Impulse);
        }
        //Left camera or body rotations
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -90 * Time.deltaTime, 0);
        }
        //Right camera or body rotations
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 90 * Time.deltaTime, 0);
        }
        //rb basic character controoler

        //basic jump for the player 
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //reset
            rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }

    }
}
