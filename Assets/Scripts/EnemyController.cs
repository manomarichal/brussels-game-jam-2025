using UnityEngine;
using UnityEngine.AI;

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

    private bool _isFollowing = false;
    private Transform _player;
    private NavMeshAgent _navMeshAgent;
    private bool canAttack = true;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.enabled = false;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No object with tag Player found");
        }
    }


    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        bool hasLineOfSight = CheckLineOfSight();

        if (!_isFollowing && distanceToPlayer <= _detectionRange && hasLineOfSight)
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

    private void FollowPlayer()
    {
        _navMeshAgent.SetDestination(_player.position);
    }

    private void StopFollowing()
    {
        _navMeshAgent.ResetPath(); 
    }

    private bool CheckLineOfSight()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, _player.position);

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
            return;
        }

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
}
