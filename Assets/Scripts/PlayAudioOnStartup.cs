using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnStartup : MonoBehaviour
{
    public AudioClip AudioClip; // Assign your audio clip in the Inspector
    [SerializeField] private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = AudioClip;
        _audioSource.Play();
        Destroy(gameObject, AudioClip.length); // Destroy the object after the audio clip duration
    }
}
