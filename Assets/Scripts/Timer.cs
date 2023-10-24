using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public SettingManager settingManager;
    [SerializeField] float currentTime; 
    public float sessionDuration;
    public float previousDuration;
    public int sessionCount = 0;
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
    public CabbageManager cabbageManager;

    [Space]
    public UnityEngine.UI.Button badButton;
    public UnityEngine.UI.Button hardButton;
    public UnityEngine.UI.Button goodButton;
    public UnityEngine.UI.Button greatButton;

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
    public float greatMultiplier = 2;
    public float goodMultiplier = 1;
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
        previousDuration = sessionDuration;

        GetComponent();
    }

    private void OnApplicationFocus(bool focus)
    {
        if(currentState == State.focus || currentState == State.rest)
        {
            switch (focus)
            {
                case false:
                    AutoResize.MinimizeWindow();
                    break;
            }
        }
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
                        AutoResize.ShowWindow();

                        ratingScreen_Result.UpdateResult();
                        timerScreen_Canvas.enabled = false;
                        ratingScreen_Canvas.enabled = true;
                        previousDuration = sessionDuration;
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
                        AutoResize.ShowWindow();
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
        sessionCount = 0;
    }
    public void ResetTimer(float nextTime)
    {
        alarmer.StopSound();
        currentState = State.pause;
        currentTime = nextTime;
        sessionDuration = currentTime;
        sessionCount = 0;
    }

    void Continue(State state)
    {
        timerScreen_Canvas.enabled = true;
        ratingScreen_Canvas.enabled = false;
        ToggleTimer(state);
        alarmer.StopSound();
    }

    void UndoTimer()
    {
        sessionDuration = previousDuration;
        ToggleTimer();
        cabbageManager.UndoCabbage();

        ratingScreen_Result.UpdateResult();
        timerScreen_Canvas.enabled = false;
        ratingScreen_Canvas.enabled = true;
    }    

    void UpdateShortcut()
    {
        if(timerScreen_Canvas.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Space)) ToggleTimer();

            if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
            {
                if(sessionCount > 0)
                {
                    UndoTimer();
                }
            }
        }
        if(ratingScreen_Canvas.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) badButton.onClick.Invoke();
            if (Input.GetKeyDown(KeyCode.Alpha2)) hardButton.onClick.Invoke();
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Space)) goodButton.onClick.Invoke();
            if (Input.GetKeyDown(KeyCode.Alpha4)) greatButton.onClick.Invoke();
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
                    else if (next > maxTime) ResetTimer(maxTime);
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

                    Debug.Log(previousSession);
                    ShowIndicator(1);
                    Continue(State.focus);
                    break;
                }
            case Rating.good:
                {
                    float next = sessionDuration;
                    ResetTimer(next);

                    Debug.Log(previousSession);
                    ShowIndicator(2);
                    Continue(State.focus);
                    break;
                }
            case Rating.great:
                {
                    float next = sessionDuration * greatMultiplier;
                    if (next > maxTime) ResetTimer(maxTime);
                    else ResetTimer(next);

                    Debug.Log(previousSession);
                    ShowIndicator(3);
                    Continue(State.focus);
                    break;
                }
        }
        sessionCount++;
    }
}
