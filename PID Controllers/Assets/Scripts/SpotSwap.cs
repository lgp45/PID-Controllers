using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotSwap : MonoBehaviour
{
    public Transform[] swapLocations;
    public int currentLocation = 0;
    public GameObject mover;
    public bool rotate = false;
    // Update is called once per frame
    void Update()
    {
        if(rotate == false)
        {
            mover.transform.position = Vector3.Lerp(mover.transform.position, swapLocations[currentLocation].position, Time.deltaTime);
        }
        else if (rotate == true)
        {
            mover.transform.rotation = Quaternion.Lerp(mover.transform.rotation, swapLocations[currentLocation].rotation, Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(currentLocation < swapLocations.Length - 1)
        {
            currentLocation++;
        }
        else
        {
            currentLocation = 0;
        }
    }
}
