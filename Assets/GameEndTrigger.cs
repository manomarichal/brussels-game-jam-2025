using UnityEngine;

public class GameEndTrigger : MonoBehaviour
{
    private Baby _baby;
    private InputLogic _inputLogic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if(other.GetComponent<InputLogic>())
            _inputLogic = other.GetComponent<InputLogic>();

        if (other.GetComponent<Baby>())
            _baby = other.GetComponent<Baby>();

        if(_baby != null && _inputLogic!= null)
            GameManager.Instance.GameEnding = GameEndings.Success;
    }
}

