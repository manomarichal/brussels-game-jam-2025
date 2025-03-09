using UnityEngine;

public class DamageOnEnter : MonoBehaviour
{
    [SerializeField] private int attackDamage = 999;

    public bool respawnAtCheckpoint = true;

    public bool enabled = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled || other.isTrigger)
        {
            return;
        }
     
            var health = other.GetComponent<Health>();

            if (health != null)
            {
                health.HealthDamaged(attackDamage,gameObject);
            }

            if (respawnAtCheckpoint)
            {
                var respawnAtCheckpoint = other.GetComponent<RespawnAtCheckpoint>();
                if(respawnAtCheckpoint != null) 
                     respawnAtCheckpoint.Respawn();
            }
    }
}
