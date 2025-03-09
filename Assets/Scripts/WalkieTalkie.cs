using UnityEngine;
using UnityEngine.UI;

public class WalkieTalkie : MonoBehaviour
{
    public Sprite RingingWalkie; // Sprite for ringing state
    public Sprite QuitWalkie;    // Sprite for quit state

    private AudioSource walkieSound;
    private Image walkieImage;

    void Start()
    {
        walkieSound = GetComponent<AudioSource>();
        walkieImage = GetComponent<Image>();
    }

    public void Ring()
    {
        if (walkieSound != null)
        {
            walkieSound.Play();
        }
        if (walkieImage != null && RingingWalkie != null)
        {
            walkieImage.sprite = RingingWalkie;
        }
    }

    public void QuitRinging()
    {
        if (walkieSound != null)
        {
            walkieSound.Stop();
        }
        if (walkieImage != null && QuitWalkie != null)
        {
            walkieImage.sprite = QuitWalkie;
        }
    }
}
