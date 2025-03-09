using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameEndings _gameEnding = GameEndings.None;


    public GameEndings GameEnding
    {
        get { return _gameEnding; }
        set
        {
            if (_gameEnding != value)
            _gameEnding = value;

            if (value != GameEndings.None)
                StartCoroutine(GameEnd());



        }
    }

    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            GameManager.Instance = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator GameEnd()
    {
        float timeSpeed = 0.2f;

        while(timeSpeed < 0.9f)
        {
            Time.timeScale = timeSpeed;
            timeSpeed = Mathf.Lerp(timeSpeed, 1f, 0.05f);
            yield return null;
        }
        Time.timeScale = 1;


        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}


public enum GameEndings
{
    Burning,
    Abandonment,
    // player death or child left alone for too long
    Monsters,
    Spikes,
    Success,
    None
}
