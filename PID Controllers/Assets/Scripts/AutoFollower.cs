using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(ComboPID))]
public class AutoFollower : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] ComboPID controller;
    Vector3 pushTar;
    Vector3 rotTar;
    public Transform targetPos;
    public float power;
    public float rotPower;
    public float distanceFromTarget;
    public float stoppingDistance = 0.1f;
    public bool reachedTarget = false;
    [Header("Params Control for Testing")]  //allows for minute control over the specific axis being used -- ADDITIONALPARAMS could be configured to allow this to affect specific position and rotation axis while BOTH are enabled.
    [SerializeField] bool positionalCorrection = true;
    [SerializeField] bool rotationalCorrection = false;
    public TMP_Text title;
    private void Start()
    {
        if(GetComponentInChildren<TMP_Text>() != null)
        {
            title = GetComponentInChildren<TMP_Text>();
            title.text = gameObject.name;
        }
    }
    private void Update()
    { 
        distanceFromTarget = Vector3.Distance(rb.position, targetPos.position);
        if (positionalCorrection && !reachedTarget)
        {
            pushTar = controller.PositionCalculation(Time.fixedDeltaTime, rb.position, targetPos.position);
            rb.AddForce(pushTar * power);
            // Check if the object has reached the target
            if (distanceFromTarget <= stoppingDistance)
            {
                reachedTarget = true;
                rb.velocity = Vector3.zero; // Stop applying forces
            }
        }
        else if(distanceFromTarget > stoppingDistance)
        {
            reachedTarget = false;
        }    
        if (rotationalCorrection)
        {
            rotTar = controller.RotationCalculation(Time.fixedDeltaTime, rb.rotation.eulerAngles, targetPos.rotation.eulerAngles);
            rb.AddTorque(rotTar * rotPower);
        }
    }
}
