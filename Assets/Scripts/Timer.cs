using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.UIElements;
using System.Xml.Serialization;

public class Timer : MonoBehaviour
{
    public SettingManager settingManager;
    [SerializeField] float currentTime; 
    public float sessionDuration;
    public TMP_Text currentTimeText;
    public TMP_Text currentStateText_Default;
    public TMP_Text currentStateText_Plant;
    [Serializable] public enum Rating {bad, hard, good, great};
    [Serializable] public enum State { focus, rest, pause};
    [SerializeField] Rating previousSession;
    public State currentState = State.focus;
    [SerializeField] State previousState;
    [Space]
    public GameObject timerScreen;
    public GameObject ratingScreen;
    public GameObject startScreen;
    public GameObject settingScreen;
    public GameObject PlantMode;
    public GameObject DefaultMode;

    [Space]
    public GameObject[] ratingIndicator = new GameObject[4];
    public int indicatorShowDuration = 1;
    bool showingIndicator = false;

    [Header("Setting")]
    public float startDuration = 60 * 5;
    public float restTime = 60 * 5; //default 5 min
    public float maxTime = 60 * 60; //default 1 hr
    public float minTime = 60 * 5; //
    [Space]
    public bool noSecond = false;
    public bool plant = false;
    [Header("Algorithm")]
    public float greatMultiplier = 3;
    public float goodMultiplier = 2;
    public float hardDivider = 2;
    public float badDivider = 4;

    //Private things
    Alarm alarmer;
    Result ratingScreen_Result;
    Canvas timerScreen_Canvas;
    Canvas ratingScreen_Canvas;

    void GetComponent()
    {
        alarmer = gameObject.GetComponent<Alarm>();

        ratingScreen_Result = ratingScreen.GetComponent<Result>();
        ratingScreen_Canvas = ratingScreen.GetComponent<Canvas>();
        timerScreen_Canvas = timerScreen.GetComponent<Canvas>();
    }
    void Start()
    {
        currentTime = startDuration;
        sessionDuration = currentTime;

        GetComponent();
    }


    void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        UpdateUI(time);
        UpdateShortcut();

        switch (currentState) 
        {
            case State.focus:
                {
                    currentTime -= Time.deltaTime;
                    if (currentTime < 0)
                    {
                        ToggleTimer();
                        alarmer.PlaySound("alarm");

                        ratingScreen_Result.UpdateResult();
                        timerScreen_Canvas.enabled = false;
                        ratingScreen_Canvas.enabled = true;
                    }
                    break;
                }
            case State.rest:
                {
                    currentTime -= Time.deltaTime;
                    if (currentTime < 0)
                    {
                        ToggleTimer();
                        alarmer.PlaySound("restend");
                        currentTime = sessionDuration;
                        currentState = State.focus;
                    }
                    break;
                }
            case State.pause:
                {
                    break;
                }
        }

        switch(showingIndicator)
        {
            case true:
                if (currentTime < sessionDuration - indicatorShowDuration) ShowIndicator(-1);
                break;
            case false:
                break;
        }
    }

    void UpdateUI(TimeSpan time)
    {
        switch (currentStateText_Default.IsActive()) { case true: currentStateText_Default.text = currentState.ToString(); break; }
        switch (currentStateText_Plant.IsActive()) { case true: currentStateText_Plant.text = currentState.ToString(); break; }

        switch (plant)
        {
            case true:
                PlantMode.SetActive(true);
                DefaultMode.SetActive(false);
                currentTimeText.gameObject.SetActive(false);
                break;
            case false:
                PlantMode.SetActive(false);
                DefaultMode.SetActive(true);
                currentTimeText.gameObject.SetActive(true);

                switch(currentState != State.pause)
                {
                    case true:
                        switch (noSecond)
                        {
                            case true:
                                currentTimeText.text = time.Hours.ToString("00") + ":" + time.Minutes.ToString("00");
                                break;
                            case false:
                                currentTimeText.text = time.Hours.ToString("00") + ":" + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
                                break;
                        }
                        break;
                    case false:
                        currentTimeText.text = "Pause";
                        currentStateText_Default.text = "I'm waiting.";
                        break;
                }
                break;
        }
    }

    public void ToggleTimer()
    {
        switch(currentState)
        {
            case State.focus:
                previousState = State.focus;
                currentState = State.pause;
                break;
            case State.rest:
                previousState = State.rest;
                currentState = State.pause;
                break;
            case State.pause:
                currentState = previousState;
                previousState = State.pause;
                break;
        }
    }
    public void ToggleTimer(State state)
    {
        previousState = currentState;
        currentState = state;
    }
    public void ResetTimer()
    {
        alarmer.StopSound();
        currentState = State.pause;
        currentTime = startDuration;
        sessionDuration = currentTime;
    }
    public void ResetTimer(float nextTime)
    {
        alarmer.StopSound();
        currentState = State.pause;
        currentTime = nextTime;
        sessionDuration = currentTime;
    }

    void Continue(State state)
    {
        timerScreen_Canvas.enabled = true;
        ratingScreen_Canvas.enabled = false;
        ToggleTimer(state);
        alarmer.StopSound();
    }
    void UpdateShortcut()
    {
        if(ratingScreen_Canvas.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) NextSession("bad");
            if (Input.GetKeyDown(KeyCode.Alpha2)) NextSession("hard");
            if (Input.GetKeyDown(KeyCode.Alpha3)) NextSession("good");
            if (Input.GetKeyDown(KeyCode.Alpha4)) NextSession("great");
        }
        if(timerScreen_Canvas.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Space)) ToggleTimer();
        }
    }

    void ShowIndicator(int id)
    {
        if(id >= 0)
        {
            for(int i = 0; i < ratingIndicator.Length; i++)
            {
                ratingIndicator[i].SetActive(false);
            }
            ratingIndicator[id].SetActive(true);
            showingIndicator = true;
        }
        else
        {
            for (int i = 0; i < ratingIndicator.Length; i++)
            {
                ratingIndicator[i].SetActive(false);
            }
            showingIndicator = false;
        }
    }

    public void NextSession(string rating)
    {
        previousSession = (Rating)Enum.Parse(typeof(Rating), rating);

        switch(previousSession)
        {
            case Rating.bad:
                {
                    float next = sessionDuration / badDivider;
                    if (next < minTime) ResetTimer(minTime);
                    else ResetTimer(next);

                    currentTime = restTime;

                    Debug.Log(previousSession);
                    ShowIndicator(0);
                    Continue(State.rest);
                    break;
                }
            case Rating.hard: 
                {
                    float next = sessionDuration / hardDivider;
                    if (next < minTime) ResetTimer(minTime);
                    else ResetTimer(next);

                    currentTime = restTime;

                    Debug.Log(previousSession);
                    ShowIndicator(1);
                    Continue(State.rest);
                    break;
                }
            case Rating.good:
                {
                    float next = sessionDuration * goodMultiplier;
                    if (next < minTime) ResetTimer(minTime);
                    else if (next > maxTime) ResetTimer(maxTime);
                    else ResetTimer(next);

                    Debug.Log(previousSession);
                    ShowIndicator(2);
                    Continue(State.focus);
                    break;
                }
            case Rating.great:
                {
                    float next = sessionDuration * greatMultiplier;
                    if (next < minTime) ResetTimer(minTime);
                    else if (next > maxTime) ResetTimer(maxTime);
                    else ResetTimer(next);

                    Debug.Log(previousSession);
                    ShowIndicator(3);
                    Continue(State.focus);
                    break;
                }
        }
    }
}
