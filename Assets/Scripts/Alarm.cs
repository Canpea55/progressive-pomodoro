using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alarm : MonoBehaviour
{
    public double goalTime;
    public double musicDuration;

    public AudioClip alarm_loop;
    public AudioClip alarm_finisher;
    public AudioClip restend;
    [Space]
    public AudioSource audioSource;
    public Slider volumeSlider;

    [SerializeField] bool isAlarming = false;

    private void Start()
    {
        audioSource.volume = volumeSlider.value;
        
    }
    public void UpdateVolume()
    {
        audioSource.volume = volumeSlider.value;
    }

    private void Update()
    {
        if (isAlarming)
        {
            switch(Application.isFocused)
            {
                case true:
                    if(AudioSettings.dspTime >= goalTime)
                    {
                        audioSource.clip = alarm_finisher;
                        audioSource.PlayScheduled(goalTime);
                        isAlarming = false;
                    }
                    break;
                case false:
                    if (AudioSettings.dspTime >= goalTime)
                    {
                        audioSource.clip = alarm_loop;
                        audioSource.PlayScheduled(goalTime);

                        musicDuration = (double)alarm_loop.samples / alarm_loop.frequency;
                        goalTime = goalTime + musicDuration;
                    }
                    break;
            }
        }
    }

    public void PlaySound(string type)
    {
        goalTime = AudioSettings.dspTime;

        switch(type)
        {
            case "alarm":
                switch(Application.isFocused)
                {
                    case true:
                        audioSource.clip = alarm_finisher;
                        audioSource.PlayScheduled(goalTime);
                        break;
                    case false:
                        audioSource.clip = alarm_loop;
                        audioSource.PlayScheduled(goalTime);

                        musicDuration = (double)alarm_loop.samples / alarm_loop.frequency;
                        goalTime = goalTime + musicDuration;
                        isAlarming = true;
                        break;
                }
                break;

            case "restend":
                audioSource.clip = restend;
                audioSource.PlayScheduled(goalTime);
                isAlarming = false;
                break;

            default:
                Debug.Log("what");
                break;
        }
    }

    public void StopSound()
    {
        audioSource.Stop();
        isAlarming = false;
    }
}
