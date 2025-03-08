using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Baby : MonoBehaviour, IEquipment
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Rigidbody _rb;


    public void DropItem()
    {
        transform.parent = null;
        _agent.enabled = true;
        _rb.isKinematic = false;

    }

    public void EquipItem(Health carryer)
    {
        transform.SetParent(carryer.AttachmentPoint.transform,false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity; 

        _agent.enabled = false;
        _rb.isKinematic = true;

    }

    public void Throw(Vector3 position, Vector3 Direction)
    {
        transform.SetParent(null);
        transform.position = position + Direction.normalized;


        _agent.enabled = true;
        _rb.isKinematic = false;

        _rb.AddForce(Direction, ForceMode.Impulse);

    }

    public void UseItem()
    {
        if (transform.parent != null)
        {

            StopAllCoroutines();
            StartCoroutine(SootheBaby());

        }
    }

    public IEnumerator SootheBaby()
    {
        Animator animator = transform.parent.GetComponent<Animator>();

        animator.SetBool("IsSoothing",true);

        yield return new WaitForSeconds(3f);
        animator.SetBool("IsSoothing", false);

    }
}
