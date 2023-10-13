using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateColor : MonoBehaviour
{
    [SerializeField]TMP_Text state;

    private void Start()
    {
        state = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        switch(state.text)
        {
            case "focus":
                state.faceColor = new Color(173, 216, 230); //rightblue
                break;
            case "rest":
                state.color = new Color(242, 140, 40); //CadmiumOrange
                break;
            case "pause":
                state.color = Color.white;
                break;
        }
        
    }
}
