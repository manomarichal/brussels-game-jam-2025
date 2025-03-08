using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class RespawnAtCheckpoint : MonoBehaviour
{
    private Vector3 respawnPosition;
    private Quaternion respawnRotation;

    private List<Transform> checkpoints = new List<Transform>();

    [SerializeField] private Transform playerTransform;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Fader fader;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Checkpoints"))
        {
            return;
        }

        if (!checkpoints.Contains(other.transform))
        {
            checkpoints.Add(other.transform);
            respawnPosition = playerTransform.position; 
            respawnRotation = playerTransform.rotation; 
            Debug.Log("Saved at Checkpoint: " + respawnPosition);
        }
    }

    public void Respawn()
    {
        StartCoroutine(RespawnSequence());
    }
    
    private IEnumerator RespawnSequence()
    {
        Debug.Log("Respawning at : " + respawnPosition);

        // disable movement and fade out
        characterController.enabled = false;
        yield return StartCoroutine(fader.Fade(1f, 0.5f));  
        
        // move player
        playerTransform.position = respawnPosition;
        playerTransform.rotation = respawnRotation;

        // fade in and enable movement again
        yield return StartCoroutine(fader.Fade(0f, 0.5f));  
        characterController.enabled = true;

        Debug.Log("Respawning complete!");

    }
}
