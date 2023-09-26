using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] SimplePID controller;

    public Transform targetPos;
    public float power;

    [Header("Params Control for Testing")]  //allows for minute control over the specific axis being used -- ADDITIONALPARAMS could be configured to allow this to affect specific position and rotation axis while BOTH are enabled.
    [SerializeField] bool xAxis = false;
    [SerializeField] bool yAxis = false;
    [SerializeField] bool zAxis = false;
    [SerializeField] bool positionalCorrection;
    [SerializeField] bool rotationalCorrection;
    private void Start()
    {

    }
    private void FixedUpdate()
    {
        if(positionalCorrection)
        {
            //float push = controller.PositionCalculation(Time.fixedDeltaTime, rb.position.y, targetPos.position.y); //the original -- not needed as we are doing per axis positional force corrections
            //this is intended to allow us to correct the positional either for each axis independently or in mixed configurations.
            if (xAxis)  //allows X axis positional calcs
            {
                float push = controller.PositionCalculation(Time.fixedDeltaTime, rb.position.x, targetPos.position.x); //not the original calc push
                rb.AddForce(new Vector3(push * power, 0, 0));
            }
            if (yAxis)  //allows Y axis positional calcs
            {
                float push = controller.PositionCalculation(Time.fixedDeltaTime, rb.position.y, targetPos.position.y); //not the original calc push
                rb.AddForce(new Vector3(0, push * power, 0));
            }
            if (zAxis)  //allows Z axis positional calcs
            {
                float push = controller.PositionCalculation(Time.fixedDeltaTime, rb.position.z, targetPos.position.z); //not the original calc push
                rb.AddForce(new Vector3(0, 0, push * power));
            }

        }
        if(rotationalCorrection)
        {
            //removed header line of Push calc to provide more directed control
            if(xAxis)
            {
                float push = controller.RotationCalculation(Time.fixedDeltaTime, rb.rotation.x, targetPos.rotation.x); //not the original calc push
                rb.AddTorque(new Vector3(push * power, 0, 0));
                //transform.Rotate(new Vector3(push * power, 0, 0));
            }
            if(yAxis)
            {
                float push = controller.RotationCalculation(Time.fixedDeltaTime, rb.rotation.y, targetPos.rotation.y); //not the original calc push
                rb.AddTorque(new Vector3(0, push * power, 0));
                //transform.Rotate(new Vector3(0, push * power, 0));
            }
            if(zAxis)
            {
                float push = controller.RotationCalculation(Time.fixedDeltaTime, rb.rotation.z, targetPos.rotation.z); //not the original calc push
                rb.AddTorque(new Vector3(0, 0, push * power));
                //transform.Rotate(new Vector3(0, 0, push * power));
            }
        }
    }
}
