using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderPercent : MonoBehaviour
{
    public Slider slider;
    public TMP_Text display;

    public void UpdateDisplay()
    {
        display.text = (slider.value * 100).ToString("0") + "%";
    }


}
