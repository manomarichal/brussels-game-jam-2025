using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent _onButtonActivate;
    [SerializeField] private UnityEvent _onButtonDeactivate;
    private bool IsButtonActive = false;

    [SerializeField] private Material _inactiveMaterial;
    [SerializeField] private Material _activeMaterial;
    private MeshRenderer _meshRenderer;
    
    private List<GameObject> _insideButtonTrigger = new List<GameObject>();

    public void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        _onButtonActivate.Invoke();

        if(!_insideButtonTrigger.Contains(other.gameObject))
        _insideButtonTrigger.Add(other.gameObject);

        _meshRenderer.material = _activeMaterial;
        transform.localPosition = new Vector3(0, 0.1f, 0);
    }

    
    public void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
            return;


        _insideButtonTrigger.Remove(other.gameObject);
        if (_insideButtonTrigger.Count == 0)
        {
            _onButtonDeactivate.Invoke();
            _meshRenderer.material = _inactiveMaterial;
            transform.localPosition = new Vector3(0, 0.2f, 0);
        }
    }

}
