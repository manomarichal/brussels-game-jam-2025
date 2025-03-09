using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private float normalPitch = 1;

    private void Awake()
    {
        m_AudioSource.playOnAwake = false;
    }
    public void PlaySound()
    {
        m_AudioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f)* normalPitch;
        m_AudioSource.Play();
    }
}

