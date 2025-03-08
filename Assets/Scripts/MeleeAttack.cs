using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private LayerMask attackableLayers; // Only hit specific layers (e.g., enemies)

    public void Attack()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, attackRange, attackableLayers);

        foreach (Collider hit in hitObjects)
        {
            Health targetHealth = hit.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
