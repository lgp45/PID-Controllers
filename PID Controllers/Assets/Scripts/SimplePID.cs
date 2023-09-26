using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;

public class SimplePID : MonoBehaviour
{
    public float pGain; //proportional gains -- nonmodified
    public float iGain; //integral gains -- ran through an integral version of proportional gains
    public float dGain; //derivative gains  -- ran through the derivative version of propotrional gains

    [Header("Additional Tuning Params")]
    [Tooltip("used as a base value to set the +/- range of the Integral Potentials")]
    public int integralLimiter;// used as a base value to set the +/- range

    [Header("Error Ratio Tracking")]
    public float cachedError; //the stored error value used for the repeated update check
    public float cachedValue; //the stored value used for the repeated update check
    public float cachedIntegral; //the stored integral value used for the repeated update check
    public float currentVelocity; //the stored velocity value used for the repeated update check

    float outputMin = -1;
    float outputMax = 1;

    [Header("Testing Control - Isolating Params")]  //allows for direct control over how MUCH of the PID process is allowed to run at a time essentially - how fine tuned / how good it is at correcting to its targets
    public bool proportionalGainOn = true;
    public bool integralGainOn = true;
    public bool derivativeGainOn = true;
    public bool allowDerivativeKick = true;
        
    //this will get us the correct angle to avoid conversion loss / allow us to flip around properly using euler and not quaternion math
    float AngleDifference(float a, float b) 
    {
        return (a - b + 540) % 360 - 180;   
    }
    public float RotationCalculation(float dt, float currentValue, float targetValue)
    {
        float P = 0;    //initialize and set to 0 in-case we want to isolate out params for display testing
        float I = 0;
        float D = 0;
        float error;
        error = AngleDifference(targetValue, currentValue);

        if (proportionalGainOn == true)
        {
            P = pGain * error;
        }
        if (integralGainOn == true)     //COMPUTES THE SUM OF ERROR OVER TIME -- ELIMINATES STEADY STATE ERROR VALUES
        {
            //this will force the cachedIntegral error correction to be clamped by the integralLimiters base value +/- range;
            cachedIntegral = Mathf.Clamp(cachedIntegral + (error * dt), -integralLimiter, integralLimiter);
            I = iGain * cachedIntegral;
        }
        if (derivativeGainOn == true)   //USEFUL FOR HANDLING THE OSCILIATION BEHAVIOR FROM PROPORTIONAL GAIN
        {
            //With derivative KICK still remaining 

            if (allowDerivativeKick)
            {
                float errorDelta = AngleDifference(error, cachedError) / dt;
                cachedError = error;
                D = dGain * errorDelta;

            }
            else
            {
                float posValDelta = AngleDifference(currentValue, cachedValue) / dt;
                cachedValue = currentValue;
                D = dGain * -posValDelta;

            }
        }
        cachedValue = currentValue;
        return Mathf.Clamp(P + I + D, outputMin, outputMax);
        /*//calculate the initial Error rate
        float error = AngleDifference(targetValue, currentValue);
        float P = 0, I = 0, D = 0;
        if(proportionalGainOn)
        {
            //calculate P term
            P = pGain * error;
        }
        if(integralGainOn)
        {
            //calculate I term
            cachedIntegral = Mathf.Clamp(cachedIntegral + (error * dt), -integralLimiter, integralLimiter);
            I = iGain * cachedIntegral;

        }
        if(derivativeGainOn)
        {
            //calculate both D terms
            float errorRateOfChange = AngleDifference(error, cachedError) / dt;
            cachedError = error;

            float valueRateOfChange = AngleDifference(currentValue, cachedValue) / dt;
            cachedValue = currentValue;
            currentVelocity = valueRateOfChange;

            if (allowDerivativeKick) //allowing the kick introduces some overshoot / overcorrection behavior -- sometimes this is desired so its left in as an example feature
            {
                D = dGain * errorRateOfChange;
            }
            else
            {
                D = dGain * -valueRateOfChange;
            }
        }
        cachedValue = currentValue;
        float result = P + I + D;
        return Mathf.Clamp(result, outputMin, outputMax);*/
    }
    //dt = the rate of change of frame rate or update rate (fixed update will be 1/60 or 60 frames constant
    public float PositionCalculation(float dt, float currentValue, float targetValue)
    {
        float error = targetValue - currentValue;
        float P = 0;    //initialize and set to 0 in-case we want to isolate out params for display testing
        float I = 0;
        float D = 0;
        if (proportionalGainOn == true)
        {
            P = pGain * error;
        }
        if (integralGainOn == true)     //COMPUTES THE SUM OF ERROR OVER TIME -- ELIMINATES STEADY STATE ERROR VALUES
        {
            //ALLOWS INTEGRAL WINDUP (UNDESIRED BEHAVIOR) TO HAPPEN - this happens due to the overcorrection applied when P is forced down to 0 and error rate is 0
            //CORRECTING THE INTEGRAL WINDUP OVERSHOOT BEHAVIOR FROM MASS ERROR ZEROING AND P ZEROING
            //this will force the cachedIntegral error correction to be clamped by the integralLimiters base value +/- range;
            cachedIntegral = Mathf.Clamp(cachedIntegral + (error * dt), -integralLimiter, integralLimiter);
            I = iGain * cachedIntegral;
        }
        if (derivativeGainOn == true)   //USEFUL FOR HANDLING THE OSCILIATION BEHAVIOR FROM PROPORTIONAL GAIN
        {
            //With derivative KICK still remaining 
            if(allowDerivativeKick)
            {
                float errorDelta = (error - cachedError) / dt;
                cachedError = error;
                D = dGain * errorDelta;
            }
            else
            {
                //WITHOUT derivative KICK.
                float posValDelta = (currentValue - cachedValue) / dt;
                cachedValue = currentValue;
                D = dGain * -posValDelta;
            }
        }
        var result =  P + I + D;   //we can return a simple sum of these values or we can create clamp averageing for the return value
        return Mathf.Clamp(result, outputMin, outputMax);
    }

}

