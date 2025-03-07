using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public GameObject carryhandle;

    public UnityEvent<int, int> OnHealthChanged;

    public UnityEvent OnDeath;


    public int CharHealth { 
        get { return _charHealth; }  
        set {


            if(value>_maxHealth)
                value = _maxHealth;

            if (_isDead || value == _charHealth)
                return;


            OnHealthChanged?.Invoke(value,_charHealth+value);
            _charHealth = value;

            if (_charHealth <= 0)
            {
                _isDead = true;
                _charHealth = 0;
                OnDeath?.Invoke();
            }

        }
    }

    [SerializeField] private int _maxHealth;

    public GameObject AttachmentPoint;

    private int _charHealth;

    private bool _isDead = false;

    private void Awake()
    {
        _charHealth = _maxHealth;
    }

    public void HealthDamaged(int health)
    {
        CharHealth-=health;
    }

    public void HealthHealed(int health)
    {
        CharHealth += health;
    }
}
