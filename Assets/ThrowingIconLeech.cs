using UnityEngine;
using UnityEngine.UI;

public class ThrowingIconLeech : MonoBehaviour
{
    [SerializeField] private InputLogic _player;
    [SerializeField] private Image[] _images;

    private void FixedUpdate()
    {
        float prog = _player.GetThrowProgression();
        if (prog <= 0)
        {
            foreach (var image in _images)
            {
                image.enabled = false;
            }
        }
        else
        {
            _images[0].fillAmount = prog;
            foreach (var image in _images)
            {
                image.enabled = true;
            }
        }
    }
    
}
