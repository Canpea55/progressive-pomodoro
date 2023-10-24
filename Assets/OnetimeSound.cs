using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnetimeSound : MonoBehaviour
{
    public AudioSource audioSource;

    void Update()
    {
        if(audioSource.time >= audioSource.clip.length)
        {
            Destroy(this.gameObject);
        }
    }
}
