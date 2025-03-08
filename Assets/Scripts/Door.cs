using UnityEngine;

public class Door : MonoBehaviour
{
    public Material openMaterial;
    public Material closedMaterial;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private bool isOpen = false;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        
        
        Close(); // Ensure the door starts closed
    }

    public void Open()
    {
        meshRenderer.material = openMaterial;
        meshCollider.enabled = false;
        isOpen = true;
    }

    public void Close()
    {
        meshRenderer.material = closedMaterial;
        meshCollider.enabled = true;
        isOpen = false;
    }
}