using UnityEngine;
using UnityEngine.Rendering;

public class FramerateManager : MonoBehaviour
{
    public int targetFPS = 90;
    public float interactionInterval = 3; //delay time in second before turndown fps
    [Space]
    public int pausingInterval = 5;
    public int saveInterval = 3;
    public int fullInterval = 1;
    private float lastInterection;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;

        lastInterection = interactionInterval;
    }

    void Update()
    {
        if(Application.targetFrameRate != targetFPS) Application.targetFrameRate = targetFPS;

        if ((Input.anyKey || Input.mouseScrollDelta != new Vector2(0,0)) || (Input.touchCount > 0))
        {
            lastInterection = interactionInterval;
        }

        if (lastInterection <= 0)
        {   
            SaveMode();
        }
        else
        {
            PerformanceMode();
            lastInterection -= Time.deltaTime;
        }
    }

    void SaveMode()
    {
        OnDemandRendering.renderFrameInterval = saveInterval;
    }
    void PerformanceMode()
    {
        OnDemandRendering.renderFrameInterval = fullInterval;
    }
}