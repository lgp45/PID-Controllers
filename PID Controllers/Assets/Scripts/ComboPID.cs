using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboPID : MonoBehaviour
{
    [Header("PID Positional Tuning Params")]
    public Vector3 pGain; //proportional gains -- nonmodified
    public Vector3 iGain; //integral gains -- ran through an integral version of proportional gains
    public Vector3 dGain; //derivative gains  -- ran through the derivative version of propotrional gains
    [Header("PID Rotation Tuning Params")]
    public Vector3 pGainRot; //proportional gains -- nonmodified
    public Vector3 iGainRot; //integral gains -- ran through an integral version of proportional gains
    public Vector3 dGainRot; //derivative gains  -- ran through the derivative version of propotrional gains

    [Header("Additional Tuning Params")]
    [Tooltip("used as a base value to set the +/- range of the Integral Potentials")]
    [SerializeField] int integralLimiter;// used as a base value to set the +/- range

    [Header("Error Ratio Tracking for POSITION")]
    [SerializeField] Vector3 cachedError; //the stored error value used for the repeated update check
    [SerializeField] Vector3 cachedValue; //the stored value used for the repeated update check
    [SerializeField] Vector3 cachedIntegral; //the stored integral value used for the repeated update check

    [Header("Error Ratio Tracking for Rotations")]
    [SerializeField] Vector3 cachedErrorRot; //the stored error value used for the repeated update check
    [SerializeField] Vector3 cachedValueRot; //the stored value used for the repeated update check
    [SerializeField] Vector3 cachedIntegralRot; //the stored integral value used for the repeated update check

    int outputMin = -1;
    int outputMax = 1;

    [Header("Testing Control - Isolating Params")]  //allows for direct control over how MUCH of the PID process is allowed to run at a time essentially - how fine tuned / how good it is at correcting to its targets
    public bool proportionalGainOn = true;
    public bool integralGainOn = true;
    public bool derivativeGainOn = true;
    public bool allowDerivativeKick = true;

    float AngleDifference(float a, float b)
    {
        return (a - b + 540) % 360 - 180;
    }
    //dt = the rate of change of frame rate or update rate (fixed update will be 1/60 or 60 frames constant
    public Vector3 PositionCalculation(float dt, Vector3 currentValue, Vector3 targetValue)
    {
        Vector3 error;
        error.x = targetValue.x - currentValue.x;
        error.y = targetValue.y - currentValue.y;
        error.z = targetValue.z - currentValue.z;
        float Px = 0, Py = 0, Pz = 0;    //initialize and set to 0 in-case we want to isolate out params for display testing
        float Ix = 0, Iy = 0, Iz = 0;
        float Dx = 0, Dy = 0, Dz = 0;
        if (proportionalGainOn == true)
        {
            Px = pGain.x * error.x;
            Py = pGain.y * error.y;
            Pz = pGain.z * error.z;
        }
        if (integralGainOn == true)     //COMPUTES THE SUM OF ERROR OVER TIME -- ELIMINATES STEADY STATE ERROR VALUES
        {
            //this will force the cachedIntegral error correction to be clamped by the integralLimiters base value +/- range;
            cachedIntegral.x = Mathf.Clamp(cachedIntegral.x + (error.x * dt), -integralLimiter, integralLimiter);
            Ix = iGain.x * cachedIntegral.x;
            cachedIntegral.y = Mathf.Clamp(cachedIntegral.y + (error.y * dt), -integralLimiter, integralLimiter);
            Iy = iGain.y * cachedIntegral.y;
            cachedIntegral.z = Mathf.Clamp(cachedIntegral.z + (error.z * dt), -integralLimiter, integralLimiter);
            Iz = iGain.z * cachedIntegral.z;
        }
        if (derivativeGainOn == true)   //USEFUL FOR HANDLING THE OSCILIATION BEHAVIOR FROM PROPORTIONAL GAIN
        {
            //With derivative KICK still remaining 
            if (allowDerivativeKick)
            {
                float errorDeltaX = (error.x - cachedError.x) / dt;
                cachedError.x = error.x;
                Dx = dGain.x * errorDeltaX;
                float errorDeltaY = (error.y - cachedError.y) / dt;
                cachedError.y = error.y;
                Dy = dGain.y * errorDeltaY;
                float errorDeltaZ = (error.z - cachedError.z) / dt;
                cachedError.z = error.z;
                Dz = dGain.z * errorDeltaZ;
            }
            else
            {
                float posValDeltaX = (currentValue.x - cachedValue.x) / dt;
                cachedValue.x = currentValue.x;
                Dx = dGain.x * -posValDeltaX;
                float posValDeltaY = (currentValue.y - cachedValue.y) / dt;
                cachedValue.y = currentValue.y;
                Dy = dGain.y * -posValDeltaY;
                float posValDeltaZ = (currentValue.z - cachedValue.z) / dt;
                cachedValue.z = currentValue.z;
                Dz = dGain.z * -posValDeltaZ;
            }
        }

        return new Vector3(Mathf.Clamp(Px + Ix + Dx, outputMin, outputMax), Mathf.Clamp(Py + Iy + Dy, outputMin, outputMax), Mathf.Clamp(Pz + Iz + Dz, outputMin, outputMax));
    }
    public Vector3 RotationCalculation(float dt, Vector3 currentValue, Vector3 targetValue)
    {

        float Px = 0, Py = 0, Pz = 0;    //initialize and set to 0 in-case we want to isolate out params for display testing
        float Ix = 0, Iy = 0, Iz = 0;
        float Dx = 0, Dy = 0, Dz = 0;
        Vector3 errorRot;
        errorRot.x = AngleDifference(targetValue.x, currentValue.x);
        errorRot.y = AngleDifference(targetValue.y, currentValue.y);
        errorRot.z = AngleDifference(targetValue.z, currentValue.z);
        if (proportionalGainOn == true)
        {
            Px = pGainRot.x * errorRot.x;
            Py = pGainRot.y * errorRot.y;
            Pz = pGainRot.z * errorRot.z;
        }
        if (integralGainOn == true)     //COMPUTES THE SUM OF ERROR OVER TIME -- ELIMINATES STEADY STATE ERROR VALUES
        {
            //this will force the cachedIntegral error correction to be clamped by the integralLimiters base value +/- range;
            cachedIntegralRot.x = Mathf.Clamp(cachedIntegralRot.x + (errorRot.x * dt), -integralLimiter, integralLimiter);
            Ix = iGainRot.x * cachedIntegralRot.x;
            cachedIntegralRot.y = Mathf.Clamp(cachedIntegralRot.y + (errorRot.y * dt), -integralLimiter, integralLimiter);
            Iy = iGainRot.y * cachedIntegralRot.y;
            cachedIntegralRot.z = Mathf.Clamp(cachedIntegralRot.z + (errorRot.z * dt), -integralLimiter, integralLimiter);
            Iz = iGainRot.z * cachedIntegralRot.z;
        }
        if (derivativeGainOn == true)   //USEFUL FOR HANDLING THE OSCILIATION BEHAVIOR FROM PROPORTIONAL GAIN
        {
            //With derivative KICK still remaining 

            if (allowDerivativeKick)
            {
                float errorDeltaX = AngleDifference(errorRot.x, cachedErrorRot.x) / dt;
                cachedErrorRot.x = errorRot.x;
                Dx = dGainRot.x * errorDeltaX;
                float errorDeltaY = AngleDifference(errorRot.y, cachedErrorRot.y) / dt;
                cachedErrorRot.y = errorRot.y;
                Dy = dGainRot.y * errorDeltaY;
                float errorDeltaZ = AngleDifference(errorRot.z, cachedErrorRot.z) / dt;
                cachedErrorRot.z = errorRot.z;
                Dz = dGainRot.z * errorDeltaZ;
            }
            else
            {
                float posValDeltaX = AngleDifference(currentValue.x, cachedValueRot.x) / dt;
                cachedValueRot.x = currentValue.x;
                Dx = dGainRot.x * -posValDeltaX;
                float posValDeltaY = AngleDifference(currentValue.y, cachedValueRot.y) / dt;
                cachedValueRot.y = currentValue.y;
                Dy = dGainRot.y * -posValDeltaY;
                float posValDeltaZ = AngleDifference(currentValue.z, cachedValueRot.z) / dt;
                cachedValueRot.z = currentValue.z;
                Dz = dGainRot.z * -posValDeltaZ;
            }
        }
        cachedValueRot = currentValue;
        return new Vector3(Mathf.Clamp(Px + Ix + Dx, outputMin, outputMax), Mathf.Clamp(Py + Iy + Dy, outputMin, outputMax), Mathf.Clamp(Pz + Iz + Dz, outputMin, outputMax));
    }
}

