using System.Collections;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    public float interval = 2f; // Time in seconds between toggles
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
            }
            else
            {
                fireParticles.Stop();
                trapCollider.enabled = false;
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
