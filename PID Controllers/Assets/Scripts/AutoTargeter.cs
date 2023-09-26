using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(SimplePID))]
public class AutoTargeter : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] SimplePID controller;

    public Transform targetPos;
    public float power; public TMP_Text title;
    private void Start()
    {
        controller = GetComponent<SimplePID>();
        if (GetComponentInChildren<TMP_Text>() != null)
        {
            title = GetComponentInChildren<TMP_Text>();
            title.text = gameObject.name;
        }
    }
    private void FixedUpdate()
    {
        var targetPosition = targetPos.position;
        targetPosition.y = rb.position.y;    //ignore difference in Y
        var targetDir = (targetPosition - rb.position).normalized;
        var forwardDir = rb.rotation * Vector3.forward;
        var currentAngle = Vector3.SignedAngle(Vector3.forward, forwardDir, Vector3.up);
        var targetAngle = Vector3.SignedAngle(Vector3.forward, targetDir, Vector3.up);

        //removed header line of Push calc to provide more directed control
        float push = controller.RotationCalculation(Time.fixedDeltaTime, currentAngle, targetAngle); //not the original calc push
        rb.AddTorque(new Vector3(0, push * power, 0));

    }
}
 