using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ComboPID))]
public class TerrainAutoLeveling : MonoBehaviour
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
    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 3f);
        {
            if (positionalCorrection)
            {
                pushTar = controller.PositionCalculation(Time.fixedDeltaTime, transform.position, hit.point + Vector3.up);
                rb.AddForce(pushTar * power);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, Time.deltaTime);
        }
    }
}
