using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ComboPID))]
public class ComboFloater : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] ComboPID controller;
    Vector3 pushTar;
    Vector3 rotTar;
    public Transform targetPos;
    public float power;
    public float rotPower;

    [Header("Params Control for Testing")]  //allows for minute control over the specific axis being used -- ADDITIONALPARAMS could be configured to allow this to affect specific position and rotation axis while BOTH are enabled.
    [SerializeField] bool positionalCorrection = true;
    [SerializeField] bool rotationalCorrection = false;
    public TMP_Text title;
    private void Start()
    {
        if (GetComponentInChildren<TMP_Text>() != null)
        {
            title = GetComponentInChildren<TMP_Text>();
            title.text = gameObject.name;
        }
    }
    private void FixedUpdate()
    {
        if (positionalCorrection)
        {
            pushTar = controller.PositionCalculation(Time.fixedDeltaTime, rb.position, targetPos.position);
            rb.AddForce(pushTar * power);
        }

        if (rotationalCorrection)
        {
            rotTar = controller.RotationCalculation(Time.fixedDeltaTime, rb.rotation.eulerAngles, targetPos.rotation.eulerAngles);
            rb.AddTorque(rotTar * rotPower);
        }
    }
}
