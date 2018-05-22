using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSound : Singleton<PickupSound>
{
    AudioSource m_audioSource;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        m_audioSource.Play();
    }
}
