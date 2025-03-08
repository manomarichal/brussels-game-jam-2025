using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer : MonoBehaviour
{
    public List<GameObject> InTheZone;

    private void Start()
    {


        InTheZone = new List<GameObject>();


    }




    private void OnTriggerEnter(Collider other)
    {
        if(!InTheZone.Contains(other.gameObject) && !other.isTrigger)
            InTheZone.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
       if(InTheZone.Contains(other.gameObject))
            InTheZone.Remove(other.gameObject);
    }
   
    
}
