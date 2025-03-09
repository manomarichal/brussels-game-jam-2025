using System;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimationDisabler : MonoBehaviour
{
    [SerializeField] List<EndingContainer> _endingContainers = new List<EndingContainer>();
    void Start()
    {
        foreach (EndingContainer container in _endingContainers)
        {
            container.EnableIfEnding(GameManager.Instance.GameEnding);
        }
    }


}

[Serializable]
public class EndingContainer
{
    public GameEndings ThisEnding;

    [SerializeField] List<GameObject> _gameObjects = new List<GameObject>();

    public void EnableIfEnding(GameEndings ending)
    {
        if (ThisEnding == ending)
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.SetActive(false);
            }
        }
    }

}