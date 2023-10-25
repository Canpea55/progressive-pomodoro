using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepManager : MonoBehaviour
{
    public static void DontSleep()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    public static void AllowSleep()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }
}
