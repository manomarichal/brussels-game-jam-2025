using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitToUnityEvent : MonoBehaviour, IShootable
{
    [Header("Event")]
    [Tooltip("Event triggers whenever the object is shot")]
    [SerializeField] private UnityEvent _shotEvent;

    [SerializeField] private ParticleSystem _deathParticles;

    public void Hit()
    {
        _shotEvent?.Invoke();

        if (_deathParticles != null)
        {
            GameObject particles = GameObject.Instantiate(_deathParticles.gameObject, transform.position, Quaternion.identity);

        }

    }


}
