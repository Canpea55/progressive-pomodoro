using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinEvent : MonoBehaviour
{
    public Animator surpriseAnimator;
    public Animator portalAnimator;

    float reloadDelay = 1;

    [Space]
    public GameObject surprise;
    public GameObject surpriseButton;
    [Space]
    public int day;
    public int month;
    public static bool theDay = false;

    private void Awake()
    {
        theDay = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch(theDay)
        {
            case false:
                DateTime dateTime = DateTime.Now;
                if (dateTime.Day == day && dateTime.Month == month)
                {
                    theDay = true;
                    surpriseButton.SetActive(true);
                    Debug.Log("yo");
                }
                break;
            case true:
                switch (surpriseAnimator.GetCurrentAnimatorStateInfo(0).IsName("SurpriseSlideout"))
                {
                    case true:
                        reloadDelay -= Time.deltaTime;
                        if (reloadDelay < 0)
                        {
                            reloadDelay = 1;
                            surprise.SetActive(false);
                            surpriseButton.SetActive(false);
                            surpriseButton.SetActive(true);
                        }
                        break;
                    case false:
                        break;
                }
                break;
        }
    }

    public void ShowSurprise()
    {
        surprise.SetActive(true);
    }

    public void HideSurprise()
    {
        surprise.SetActive(false);
    }
}
