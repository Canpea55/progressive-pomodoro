using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    public Timer timer;

    Canvas startScreen;
    Canvas timerScreen;
    Canvas ratingScreen;
    Canvas settingScreen;

    float delay;
    float delayAmout = 0.1f;

    bool pauseBeforeSetting = false;

    void Start()
    {
        startScreen = timer.startScreen.GetComponent<Canvas>();
        timerScreen = timer.timerScreen.GetComponent<Canvas>();
        ratingScreen = timer.ratingScreen.GetComponent<Canvas>();
        settingScreen = timer.settingScreen.GetComponent<Canvas>();

        ReturnHome();
    }

    private void Update()
    {
        if(delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            delay = 0;

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                delay = delayAmout;
                if(startScreen.enabled == true) Application.Quit();
                else ToggleSetting();

                if(startScreen.enabled == true && settingScreen.enabled == true) ToggleSetting();
            }
        }
    }

    public void StartTimer()
    {
        startScreen.enabled = false;
        timerScreen.enabled = true;
        timer.ResetTimer();
        timer.ToggleTimer(Timer.State.focus);
    }

    public void ReturnHome()
    {
        timer.ToggleTimer(Timer.State.pause);
        startScreen.enabled = true;
        timerScreen.enabled = false;
        ratingScreen.enabled = false;
        settingScreen.enabled = false;
    }

    public void ToggleSetting()
    {
        if(timer.currentState == Timer.State.pause && settingScreen.enabled == false) { pauseBeforeSetting = true; }

        switch(pauseBeforeSetting)
        {
            case true:
                if (settingScreen.enabled == true) pauseBeforeSetting = false;
                break;

            case false:
                timer.ToggleTimer();
                break;
        }

        settingScreen.enabled = !settingScreen.enabled;
    }

}
