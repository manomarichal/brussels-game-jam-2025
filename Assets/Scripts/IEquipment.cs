using UnityEngine;

public interface IEquipment 
{
    abstract void UseItem();

    abstract void DropItem();

    abstract void Throw(Vector3 position, Vector3 Direction);

    abstract void EquipItem(Health carryer);
    

}
