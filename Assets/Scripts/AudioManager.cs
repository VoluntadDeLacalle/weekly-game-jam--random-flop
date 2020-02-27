using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioSource audSource;

    void Awake()
    {
        audSource.clip = clickSound;
    }

    public void playClick()
    {
        if (audSource.isPlaying)
        {
            audSource.Stop();
        }

        audSource.Play();
    }
}
