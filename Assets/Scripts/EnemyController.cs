using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _detectionRange = 10f;
    [SerializeField] private LayerMask _obstacleMask;


    [Header("Attack settings")]
    [SerializeField] private Vector3 attackBoxSize = new Vector3(2f, 2f, 2f); 
    [SerializeField] private Vector3 attackBoxOffset = new Vector3(0f, 0f, 1f); 
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private LayerMask attackableLayers; 
    [SerializeField] private float attackCooldown = 1f;

    [SerializeField] private Health _health;

    public UnityEvent OnAttack;
    public UnityEvent OnHit;




    private bool _isFollowing = false;
    private List<Transform> _targets = new List<Transform>();
    private NavMeshAgent _navMeshAgent;
    private bool canAttack = true;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.enabled = false;

        _health.OnHealthChanged.AddListener(HealthChange);

    }


    private void HealthChange(int newHealth, int dmgValue)
    {
        if (dmgValue > 0)
        {
            OnHit?.Invoke();
        }

    }
    void FixedUpdate()
    {


       

        if(_targets.Count > 0)
        {
            _targets = _targets.OrderBy(
               x => Vector2.Distance(this.transform.position, x.transform.position)
              ).ToList();




            bool hasLineOfSight = CheckLineOfSight();

            if (!_isFollowing && hasLineOfSight)
            {
                _isFollowing = true;
                _navMeshAgent.enabled = true;
            }

            if (_isFollowing)
            {
                if (canAttack && CheckIfTargetInRange())
                {
                    Attack();
                }
                FollowPlayer();
            }
        }
    }

    private void FollowPlayer()
    {
        _navMeshAgent.SetDestination(_targets[0].position);
    }

    private void StopFollowing()
    {
        _navMeshAgent.ResetPath(); 
    }

    private bool CheckLineOfSight()
    {
        Vector3 direction = (_targets[0].position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, _targets[0].position);

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, _obstacleMask))
        {
            return hit.collider.CompareTag("Player");
        }
        return true;
    }

    private Collider[] GetTargetsInRange()
    {
        Vector3 attackPosition = transform.position + transform.TransformDirection(attackBoxOffset);
        Collider[] hitObjects = Physics.OverlapBox(attackPosition, attackBoxSize * 0.5f, transform.rotation, attackableLayers);

        return hitObjects;
    }
    private bool CheckIfTargetInRange()
    {
        return GetTargetsInRange().Length>0;
    }
    
    public void Attack()
    {        
        Debug.Log("Attack!");

        canAttack = false;
        Invoke(nameof(ResetAttack), attackCooldown); 
        
        Collider[] hitObjects = GetTargetsInRange();
        foreach (Collider hit in hitObjects)
        {
            if (hit.isTrigger)
                continue;

            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.HealthDamaged(attackDamage, gameObject);
            }
        }
        OnAttack?.Invoke();

    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        // Draw attack box in editor for visualization
        Gizmos.color = Color.red;
        Vector3 attackPosition = transform.position + transform.TransformDirection(attackBoxOffset);
        Gizmos.matrix = Matrix4x4.TRS(attackPosition, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, attackBoxSize);
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.isTrigger)
            return;

        if (!_targets.Contains(other.transform))
            _targets.Add(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
      //  if (other.isTrigger)
      //      return;
      //
      //  if (_targets.Contains(other.transform))
      //      _targets.Remove(other.transform);
    }
}
