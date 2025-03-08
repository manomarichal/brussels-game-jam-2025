using UnityEngine;
using UnityEngine.UI;

public class HealthLeech : MonoBehaviour
{
    [SerializeField] private Health _player;
    [SerializeField] private Image _image;


    private void Start()
    {
        _player.OnHealthChanged.AddListener(HealthChange);
        _image.fillAmount = 1f;

    }


    private void HealthChange(int newHealth, int dmgValue)
    {


        _image.fillAmount = _player.GetHealthPercentage();

    }
}
