using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PIDEditor : MonoBehaviour
{
    public GameObject simplePanel;
    public GameObject comoboPanel;
    public TMP_Text simpleTitle;
    public TMP_Text comboTitle;
    public GameObject obj;
    [Header("SimplePID")]
    public TMP_InputField pGain;
    public TMP_InputField iGain;
    public TMP_InputField dGain; 
    public TMP_InputField powerSimple;

    [Header("ComboPID")]
    public TMP_InputField pGainX;
    public TMP_InputField pGainY;
    public TMP_InputField pGainZ;
    public TMP_InputField iGainX;
    public TMP_InputField iGainY;
    public TMP_InputField iGainZ;
    public TMP_InputField dGainX;
    public TMP_InputField dGainY;
    public TMP_InputField dGainZ;
    public TMP_InputField pGainRotX;
    public TMP_InputField pGainRotY;
    public TMP_InputField pGainRotZ;
    public TMP_InputField iGainRotX;
    public TMP_InputField iGainRotY;
    public TMP_InputField iGainRotZ;
    public TMP_InputField dGainRotX;
    public TMP_InputField dGainRotY;
    public TMP_InputField dGainRotZ;
    public TMP_InputField powerCombo;
    public TMP_InputField rotPowerCombo;


    private void Start()
    {

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {

                if(hit.collider.GetComponent<SimplePID>() != null || hit.collider.GetComponent<ComboPID>() != null)
                {
                    Debug.Log(hit.transform.gameObject.name);
                    obj = hit.transform.gameObject;
                }

                if(obj != null)
                {
                    if (obj.GetComponent<SimplePID>() != null)
                    {
                        SimplePID simplePID = obj.GetComponent<SimplePID>();
                        comoboPanel.SetActive(false);
                        simplePanel.SetActive(true);
                        UpdateInputs(pGain, simplePID.pGain);
                        UpdateInputs(iGain, simplePID.iGain);
                        UpdateInputs(dGain, simplePID.dGain);

                        if (obj.GetComponent<AutoTargeter>() != null)
                        {
                            AutoTargeter autoTargeter = obj.GetComponent<AutoTargeter>();
                            UpdateInputs(powerSimple, autoTargeter.power);
                            simpleTitle.text = autoTargeter.name;


                        }
                        if (obj.GetComponent<Floater>() != null)
                        {
                            Floater floater = obj.GetComponent<Floater>();
                            UpdateInputs(powerSimple, floater.power);
                            simpleTitle.text = floater.name;
                        }

                    }
                    if (obj.GetComponent<ComboPID>() != null)
                    {
                        ComboPID comboPID = obj.GetComponent<ComboPID>();
                        simplePanel.SetActive(false);
                        comoboPanel.SetActive(true);
                        UpdateInputs(pGainX, comboPID.pGain.x);
                        UpdateInputs(pGainY, comboPID.pGain.y);
                        UpdateInputs(pGainZ, comboPID.pGain.z);
                        UpdateInputs(iGainX, comboPID.iGain.x);
                        UpdateInputs(iGainY, comboPID.iGain.y);
                        UpdateInputs(iGainZ, comboPID.iGain.z);
                        UpdateInputs(dGainX, comboPID.dGain.x);
                        UpdateInputs(dGainY, comboPID.dGain.y);
                        UpdateInputs(dGainZ, comboPID.dGain.z);
                        UpdateInputs(pGainRotX, comboPID.pGainRot.x);
                        UpdateInputs(pGainRotY, comboPID.pGainRot.y);
                        UpdateInputs(pGainRotZ, comboPID.pGainRot.z);
                        UpdateInputs(iGainRotX, comboPID.iGainRot.x);
                        UpdateInputs(iGainRotY, comboPID.iGainRot.y);
                        UpdateInputs(iGainRotZ, comboPID.iGainRot.z);
                        UpdateInputs(dGainRotX, comboPID.dGainRot.x);
                        UpdateInputs(dGainRotY, comboPID.dGainRot.y);
                        UpdateInputs(dGainRotZ, comboPID.dGainRot.z);

                        if (obj.GetComponent<AutoFollower>() != null)
                        {
                            AutoFollower follower = obj.GetComponent<AutoFollower>();
                            UpdateInputs(powerCombo, follower.power);
                            UpdateInputs(rotPowerCombo, follower.rotPower);
                            comboTitle.text = follower.name;
                        }
                        if (obj.GetComponent<ComboFloater>() != null)
                        {
                            ComboFloater floater = obj.GetComponent<ComboFloater>();
                            UpdateInputs(powerCombo, floater.power);
                            UpdateInputs(rotPowerCombo, floater.rotPower);
                            comboTitle.text = floater.name;
                        }
                        if (obj.GetComponent<TerrainAutoLeveling>() != null)
                        {
                            TerrainAutoLeveling terrainAutoLeveling = obj.GetComponent<TerrainAutoLeveling>();
                            UpdateInputs(powerCombo, terrainAutoLeveling.power);
                            UpdateInputs(rotPowerCombo, terrainAutoLeveling.rotPower);
                            comboTitle.text = terrainAutoLeveling.name;
                        }
                    }
                }
                
            }
        }
        //Simple PID + Implementations
        UpdateProportionalGain(pGain.text);
        UpdateIntegralGain(iGain.text);
        UpdateDerivativeGain(dGain.text);
        UpdateSimplePIDImplementer(powerSimple.text);


        //Combo PID + Implementations
        UpdateComboProportinalGain(pGainX.text, pGainY.text, pGainZ.text, pGainRotX.text, pGainRotY.text, pGainRotZ.text);
        UpdateComboIntegralGain(iGainX.text, iGainY.text, iGainZ.text, iGainRotX.text, iGainRotY.text, iGainRotZ.text);
        UpdateComboDerivativeGain(dGainX.text, dGainY.text, dGainZ.text, dGainRotX.text, dGainRotY.text, dGainRotZ.text);
        UpdateComboPIDImplementer(powerCombo.text, rotPowerCombo.text);

    }
    bool TryParse(string text, out float value)
    {
        if (string.IsNullOrEmpty(text))
        {
            value = 0;
            return true;
        }

        return float.TryParse(text, out value);
    }
    public void UpdateProportionalGain(string text)
    {
        if (obj != null)
        {
            if (obj.GetComponent<SimplePID>() != null)
            {
                SimplePID simplePID = obj.GetComponent<SimplePID>();
                if (TryParse(text, out float value))
                {
                    pGain.text = text;
                    simplePID.pGain = value;
                }
            }
        }
    }
    public void UpdateIntegralGain(string text)
    {
        if (obj != null)
        {
            if (obj.GetComponent<SimplePID>() != null)
            {
                SimplePID simplePID = obj.GetComponent<SimplePID>();
                if (TryParse(text, out float value))
                {
                    iGain.text = text;
                    simplePID.iGain = value;
                }
            }
        }
    }

    public void UpdateDerivativeGain(string text)
    {
        if (obj != null)
        {
            if (obj.GetComponent<SimplePID>() != null)
            {
                SimplePID simplePID = obj.GetComponent<SimplePID>();
                if (TryParse(text, out float value))
                {
                    dGain.text = text;
                    simplePID.dGain = value;
                }
            }
        }
    }
    public void UpdateComboDerivativeGain(string text, string text2, string text3, string rot1, string rot2, string rot3)
    {
        if (obj != null)
        {
            if (obj.GetComponent<ComboPID>() != null)
            {
                ComboPID comboPID = obj.GetComponent<ComboPID>();
                if (TryParse(text, out float value))
                {
                    dGainX.text = text;
                    comboPID.dGain.x = value;
                }
                if (TryParse(text2, out float value2))
                {
                    dGainY.text = text2;
                    comboPID.dGain.y = value2;
                }
                if (TryParse(text3, out float value3))
                {
                    dGainZ.text = text3;
                    comboPID.dGain.z = value3;
                }
                if(TryParse(rot1, out float value4x))
                {
                    dGainRotX.text = rot1;
                    comboPID.dGainRot.x = value4x;
                }
                if (TryParse(rot2, out float value5y))
                {
                    dGainRotY.text = rot2;
                    comboPID.dGainRot.y = value5y;
                }
                if (TryParse(rot3, out float value6z))
                {
                    dGainRotZ.text = rot3;
                    comboPID.dGainRot.z = value6z;
                }
            }
        }
    }
    public void UpdateComboProportinalGain(string text, string text2, string text3, string rot1, string rot2, string rot3)
    {
        if (obj != null)
        {
            if (obj.GetComponent<ComboPID>() != null)
            {
                ComboPID comboPID = obj.GetComponent<ComboPID>();
                if (TryParse(text, out float value))
                {
                    pGainX.text = text;
                    comboPID.pGain.x = value;
                }
                if (TryParse(text2, out float value2))
                {
                    pGainY.text = text2;
                    comboPID.pGain.y = value2;
                }
                if (TryParse(text3, out float value3))
                {
                    pGainZ.text = text3;
                    comboPID.pGain.z = value3;
                }
                if(TryParse(rot1, out float value4))
                {
                    pGainRotX.text = rot1;
                    comboPID.pGainRot.x = value4;
                }
                if (TryParse(rot2, out float value5))
                {
                    pGainRotY.text = rot2;
                    comboPID.pGainRot.y = value5;
                }
                if (TryParse(rot3, out float value6))
                {
                    pGainRotZ.text = rot3;
                    comboPID.pGainRot.z = value6;
                }
            }
        }
    }
    public void UpdateComboIntegralGain(string text, string text2, string text3, string rot1, string rot2, string rot3)
    {
        if (obj != null)
        {
            if (obj.GetComponent<ComboPID>() != null)
            {
                ComboPID comboPID = obj.GetComponent<ComboPID>();
                if (TryParse(text, out float value))
                {
                    iGainX.text = text;
                    comboPID.iGain.x = value;
                }
                if (TryParse(text2, out float value2))
                {
                    iGainY.text = text2;
                    comboPID.iGain.y = value2;
                }
                if (TryParse(text3, out float value3))
                {
                    iGainZ.text = text3;
                    comboPID.iGain.z = value3;
                }
                if(TryParse(rot1 , out float value4c))
                {
                    iGainRotX.text = rot1;
                    comboPID.iGainRot.x = value4c;
                }
                if (TryParse(rot2, out float value5c))
                {
                    iGainRotY.text = rot2;
                    comboPID.iGainRot.y = value5c;
                }
                if (TryParse(rot3, out float value6c))
                {
                    iGainRotZ.text = rot3;
                    comboPID.iGainRot.z = value6c;
                }
            }
        }
    }
    public void UpdateComboPIDImplementer(string comboPower, string rotPower)
    {
        if (obj != null)
        {
            if (obj.GetComponent<AutoFollower>() != null)
            {
                AutoFollower follower = obj.GetComponent<AutoFollower>();
                if (TryParse(comboPower, out float value))
                {
                    powerCombo.text = comboPower;
                    follower.power = value;
                }
                if (TryParse(rotPower, out float value2))
                {
                    rotPowerCombo.text = rotPower;
                    follower.rotPower = value2;
                }
            }
            if (obj.GetComponent<ComboFloater>() != null)
            {
                ComboFloater floater = obj.GetComponent<ComboFloater>();
                if (TryParse(comboPower, out float value))
                {
                    powerCombo.text = comboPower;
                    floater.power = value;
                }
                if (TryParse(rotPower, out float value2))
                {
                    rotPowerCombo.text = rotPower;
                    floater.rotPower = value2;
                }
            }
            if (obj.GetComponent<TerrainAutoLeveling>() != null)
            {
                TerrainAutoLeveling terrainAutoLeveling = obj.GetComponent<TerrainAutoLeveling>();
                if (TryParse(comboPower, out float value))
                {
                    powerCombo.text = comboPower;
                    terrainAutoLeveling.power = value;
                }
                if (TryParse(rotPower, out float value2))
                {
                    rotPowerCombo.text = rotPower;
                    terrainAutoLeveling.rotPower = value2;
                }
            }
        }
    }

    public void UpdateSimplePIDImplementer(string power)
    {
        if(obj != null)
        {
            if (obj.GetComponent<AutoTargeter>() != null)
            {
                AutoTargeter autoTargeter = obj.GetComponent<AutoTargeter>();
                if (TryParse(power, out float value))
                {
                    powerSimple.text = power;
                    autoTargeter.power = value;
                }
            }
            if (obj.GetComponent<Floater>() != null)
            {
                Floater floater = obj.GetComponent<Floater>();
                if (TryParse(power, out float value))
                {
                    powerSimple.text = power;
                    floater.power = value;
                }
            }
        }

    }
    //public void UpdateSimplePIDToggles(bool pgain, bool igain, bool dgain, bool xPos, bool yPos, bool zPos, bool xRot, bool yRot, bool zRot)


    public void UpdateInputs(TMP_InputField input, float newValue)
    {
        if (input.isFocused) return;
        input.text = newValue.ToString();
    }

    public void UpdateToglge(Toggle input, bool newValue)
    {
        input.isOn = newValue;
    }

}
