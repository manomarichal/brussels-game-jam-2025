using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Baby : MonoBehaviour, IEquipment
{


    [SerializeField] private InputLogic _player;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Rigidbody _rb;


    [SerializeField] private List<InterestPoint> _interestingPoints = new List<InterestPoint>();

    private float _boredom = 0;
    [SerializeField] private float _patienceTime = 3f;

    [SerializeField] private float _timeBeforeCrying = 15f;

    [SerializeField] private float _timeBeforeAbandonment = 45f;

    public UnityEvent OnBabyDeath;

    public UnityEvent StartCrying;

    public UnityEvent StopCrying;

    [SerializeField] private Health _health;

    private GameEndings _currentGameEnding;

    private bool isDropped = false;
    private float timeSinceDropped = 0f;
    
    private bool _isStopped;
    private void Start()
    {
        _agent.destination = (transform.position);
        _health.OnHealthChanged.AddListener(HealthChange);

    }
    
    void Update()
    {
        if (isDropped)
        {
            timeSinceDropped += Time.deltaTime;
            if (timeSinceDropped > _timeBeforeCrying)
            {
                StartCrying.Invoke();
                isDropped = false; // Ensure signal is triggered only once
            }

            if (timeSinceDropped > _timeBeforeAbandonment)
            {
                GameManager.Instance.GameEnding = GameEndings.Abandonment;
            }
        }
    }

    private void HealthChange(int newHealth, int dmgValue)
    {
        if (newHealth <= 0)
        {
            OnBabyDeath?.Invoke();

            if (_health.LastDamage == null)
            {
                GameManager.Instance.GameEnding = GameEndings.Spikes;
                Debug.Log("burning ending");
                return;
            }
            else if(_health.LastDamage.layer == LayerMask.NameToLayer("Danger"))
            {
                GameManager.Instance.GameEnding = GameEndings.Burning;
                Debug.Log("burning ending");
                return;
            }
            else if (_health.LastDamage.layer == LayerMask.NameToLayer("Enemies"))
            {
                GameManager.Instance.GameEnding = GameEndings.Monsters;
                Debug.Log("enemies ending");
                return;

            }
        }
    }

    private void FixedUpdate()
    {
        if (_agent.isActiveAndEnabled && _agent.isOnNavMesh)
        {
            if (_isStopped || _agent.remainingDistance < 2.5f)
            {
                _boredom += Time.deltaTime;
                if (_boredom > _patienceTime)
                {
                    _boredom = 0;

                    bool coinIsHeads = Random.value < 0.5f;
                    if (coinIsHeads || _interestingPoints.Count==0)
                    {
                        // wander
                        _agent.SetDestination(transform.position + Random.insideUnitSphere * 10f);

                        Debug.Log("Baby wandering");


                    }
                    else
                    {
                        // investigate
                        _agent.SetDestination(_interestingPoints[Random.Range(0, _interestingPoints.Count)].gameObject.transform.position);
                        Debug.Log("Baby investigating");

                    }

                }



            }
            else
            {
                if ((_agent.destination - transform.position).magnitude > 10f)
                {
                    // if objective is too far, clear
                    _isStopped = true;
                    Debug.Log("Stopping baby");

                }

            }
        }
        else
        {
            Debug.Log("Baby agent not active");
        }
    }


    public void DropItem()
    {
        transform.parent = null;
        _agent.enabled = true;
        _rb.isKinematic = false;

        timeSinceDropped = 0f; // Reset the timer when dropped
        isDropped = true; // Mark as dropped
    }

    public void EquipItem(Health carrier)
    {
        transform.SetParent(carrier.AttachmentPoint.transform, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        _agent.enabled = false;
        _rb.isKinematic = true;
    }

    public void Throw(Vector3 position, Vector3 Direction)
    {
        transform.SetParent(null);
        transform.position = position + Direction.normalized*2;

        _rb.isKinematic = false;

        _rb.AddForce(Direction, ForceMode.Impulse);

    }

    public void UseItem()
    {
        if (transform.parent != null)
        {

            StopAllCoroutines();
            StartCoroutine(SootheBaby());
            StopCrying.Invoke(); // Invoke signal when item is picked up
            isDropped = false; // Mark as not dropped
        }
    }

    public IEnumerator SootheBaby()
    {
        Animator animator = transform.parent.GetComponent<Animator>();

        animator.SetBool("IsSoothing",true);

        yield return new WaitForSeconds(3f);
        animator.SetBool("IsSoothing", false);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(transform.parent ==null)
            _agent.enabled = true;


    }

     private void OnTriggerEnter(Collider other)
    {
        InterestPoint point = other.GetComponent<InterestPoint>();
        if (point == null || other.isTrigger)
            return;

        if(!_interestingPoints.Contains(point))
            _interestingPoints.Add(point);
    }

    private void OnTriggerExit(Collider other)
    {
        InterestPoint point = other.GetComponent<InterestPoint>();
        if (point == null || other.isTrigger)
            return;

        if (_interestingPoints.Contains(point))
            _interestingPoints.Remove(point);
    }

}
