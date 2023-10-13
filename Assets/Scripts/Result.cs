using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Result : MonoBehaviour
{
    public Timer timer;
    [Space]
    public TMP_Text focusedDuration;
    [Header("NextDurations")]
    public TMP_Text[] next = new TMP_Text[4];

    public void UpdateResult()
    {
        TimeSpan result = TimeSpan.FromSeconds(timer.sessionDuration);
        focusedDuration.text = result.Hours.ToString("00") + ":" + result.Minutes.ToString("00") + ":" + result.Seconds.ToString("00");

        TimeSpan[] nextTime = new TimeSpan[4];
        nextTime[0] = TimeSpan.FromSeconds(timer.sessionDuration / timer.badDivider);
        nextTime[1] = TimeSpan.FromSeconds(timer.sessionDuration / timer.hardDivider);
        nextTime[2] = TimeSpan.FromSeconds(timer.sessionDuration * timer.goodMultiplier);
        nextTime[3] = TimeSpan.FromSeconds(timer.sessionDuration * timer.greatMultiplier);

        for(int i = 0; i < nextTime.Length; i++)
        {
            if (nextTime[i].TotalSeconds < timer.minTime) nextTime[i] = TimeSpan.FromSeconds(timer.minTime);
            if (nextTime[i].TotalSeconds > timer.maxTime) nextTime[i] = TimeSpan.FromSeconds(timer.maxTime);

            next[i].text = "(" + 
                nextTime[i].Hours.ToString("00") + ":" + 
                nextTime[i].Minutes.ToString("00") + ":" + 
                nextTime[i].Seconds.ToString("00") + 
                ")";
        }
    }
}
