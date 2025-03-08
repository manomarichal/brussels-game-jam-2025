using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _detectionRange = 10f;
    [SerializeField] private LayerMask _obstacleMask;
    private bool _isFollowing = false;
    private Transform _player;

    private NavMeshAgent _navMeshAgent;

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
}
