using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ForwardUnityEvent : MonoBehaviour
{
    [SerializeField] List<UnityEvent> _unityEvents = new List<UnityEvent>();

    public void Invoke(int i)
    {
        _unityEvents[i]?.Invoke();
    }
}
