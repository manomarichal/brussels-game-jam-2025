using System.Collections;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    public float interval = 2f; // Time in seconds between toggles
    public AudioSource fireSound; // Audio source for the fire trap
    
    private ParticleSystem fireParticles;
    private BoxCollider trapCollider;
    private bool isActive = false;

    void Start()
    {
        fireParticles = GetComponent<ParticleSystem>();
        trapCollider = GetComponent<BoxCollider>();
        StartCoroutine(ToggleFire());
    }

    private IEnumerator ToggleFire()
    {
        while (true)
        {
            isActive = !isActive;
            if (isActive)
            {
                fireParticles.Play();
                trapCollider.enabled = true;
                if (fireSound != null) fireSound.Play();
            }
            else
            {
                fireParticles.Stop();
                trapCollider.enabled = false;
                if (fireSound != null) fireSound.Stop();
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
