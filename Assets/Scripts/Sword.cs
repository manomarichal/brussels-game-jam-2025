using UnityEngine;


public class Sword : MonoBehaviour, IEquipment
{
    [SerializeField] private Rigidbody _rb;

    [Header("Attack settings")]
    [SerializeField] private Vector3 attackBoxSize = new Vector3(2f, 2f, 2f); 
    [SerializeField] private Vector3 attackBoxOffset = new Vector3(0f, 0f, 1f); 
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private LayerMask attackableLayers; 
    [SerializeField] private float attackCooldown = 1f;

    private bool canAttack = true;

    public void DropItem()
    {
        transform.parent = null;
        _rb.isKinematic = false;

    }

    public void EquipItem(Health carryer)
    {
        transform.SetParent(carryer.AttachmentPoint.transform,false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity; 

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
            if (canAttack)
            {
                Attack();
            }
        }
    }

    private Collider[] GetTargetsInRange()
    {
        Vector3 attackPosition = transform.position + transform.TransformDirection(attackBoxOffset);
        Collider[] hitObjects = Physics.OverlapBox(attackPosition, attackBoxSize * 0.5f, transform.rotation, attackableLayers);

        return hitObjects;
    }
    
    public void Attack()
    {        
        Debug.Log("Attack!");

        canAttack = false;
        Invoke(nameof(ResetAttack), attackCooldown); 
        
        Collider[] hitObjects = GetTargetsInRange();
        foreach (Collider hit in hitObjects)
        {
            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.HealthDamaged(attackDamage);
            }
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
