using UnityEngine;
using UnityEngine.UI;

public class WalkieTalkie : MonoBehaviour
{
    public Sprite RingingWalkie; // Sprite for ringing state
    public Sprite QuitWalkie;    // Sprite for quit state

    [SerializeField] private Animator _animator;

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
            _animator.Play("PhoneRinging");
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
            _animator.Play("PhoneMute");

        }
        if (walkieImage != null && QuitWalkie != null)
        {
            walkieImage.sprite = QuitWalkie;
        }
    }
}
