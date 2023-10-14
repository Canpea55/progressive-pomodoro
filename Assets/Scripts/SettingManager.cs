using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using UnityEngine.UI;
using System;

public class SettingManager : MonoBehaviour
{
    public FramerateManager fpsManager;
    public Timer timer;
    public GameObject cabbageFactoryObj;
    [Header("settings")]
    public Slider volume;
    public TMP_InputField startDuration;
    public TMP_InputField restTime;
    public TMP_InputField maxTime;
    public TMP_InputField minTime;
    public TMP_InputField targetFPS;
    public Toggle noSecond;
    public Toggle plantMode;
    public Toggle cabbageFactory;

    private void Start()
    {
        LoadPref();
    }

    private void OnApplicationQuit()
    {
        SavePref();
    }

    public void SavePref()
    {
        //Alarm
        PlayerPrefs.SetFloat("volume", volume.value);

        //Timer
        PlayerPrefs.SetFloat("startDuration", (float)Convert.ToDouble(startDuration.text));
        PlayerPrefs.SetFloat("restTime", float.Parse(restTime.text, CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetFloat("maxTime", float.Parse(maxTime.text, CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetFloat("minTime", float.Parse(minTime.text, CultureInfo.InvariantCulture.NumberFormat));

        //Performance
        PlayerPrefs.SetInt("targetFPS", int.Parse(targetFPS.text, CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetInt("noSecond", noSecond.isOn ? 1 : 0);

        //Optionals
        PlayerPrefs.SetInt("plantMode", plantMode.isOn ? 1 : 0);
        PlayerPrefs.SetInt("cabbageFactory", plantMode.isOn ? 1 : 0);

        PlayerPrefs.Save();
        Debug.Log("SavePref");
    }

    public void LoadPref()
    {
        //alarm
        if (PlayerPrefs.HasKey("volume")) volume.value = PlayerPrefs.GetFloat("volume"); else volume.value = 1;

        //timer
        if (PlayerPrefs.HasKey("startDuration")) startDuration.text = PlayerPrefs.GetFloat("startDuration").ToString(); else startDuration.text = "300";
        if (PlayerPrefs.HasKey("restTime")) restTime.text = PlayerPrefs.GetFloat("restTime").ToString(); else restTime.text = "300";
        if (PlayerPrefs.HasKey("maxTime")) maxTime.text = PlayerPrefs.GetFloat("maxTime").ToString(); else maxTime.text = "3600";
        if (PlayerPrefs.HasKey("minTime")) minTime.text = PlayerPrefs.GetFloat("minTime").ToString(); else minTime.text = "300";

        //Performance
        if (PlayerPrefs.HasKey("targetFPS")) targetFPS.text = PlayerPrefs.GetInt("targetFPS").ToString(); else targetFPS.text = "90";
        noSecond.isOn = Int2Bool(PlayerPrefs.GetInt("noSecond"));

        //optionals
        plantMode.isOn = Int2Bool(PlayerPrefs.GetInt("plantMode"));
        plantMode.isOn = Int2Bool(PlayerPrefs.GetInt("cabbageFactory"));

        Debug.Log("LoadPref");
    }

    bool Int2Bool(int val)
    {
        switch(val)
        {
            case 1:
                return true;
            case 2:
                return false;
            default: return false;
        }
    }

    public float GetPref(string setting)
    {
        switch(setting)
        {
            case "volume":
                return volume.value;
                
            case "startDuration":
                return float.Parse(startDuration.text, CultureInfo.InvariantCulture.NumberFormat);
               
            case "restTime":
                return float.Parse(restTime.text, CultureInfo.InvariantCulture.NumberFormat);
                
            case "maxTime":
                return float.Parse(maxTime.text, CultureInfo.InvariantCulture.NumberFormat);
               
            case "minTime":
                return float.Parse(minTime.text, CultureInfo.InvariantCulture.NumberFormat);
            
            case "targetFPS":
                return int.Parse(targetFPS.text, CultureInfo.InvariantCulture.NumberFormat);


            default:
                Debug.LogError("Can't get pref");
                return 0;
        }
    }

    public void UpdateSettings()
    {
        //Timer
        timer.startDuration = GetPref("startDuration");
        timer.restTime = GetPref("restTime");
        timer.maxTime = GetPref("maxTime");
        timer.minTime = GetPref("minTime");

        //Performance
        fpsManager.targetFPS = (int)GetPref("targetFPS");
        timer.noSecond = noSecond.isOn;

        //Optionals
        timer.plant = plantMode.isOn;
        cabbageFactoryObj.SetActive(cabbageFactory.isOn);
    }
}
