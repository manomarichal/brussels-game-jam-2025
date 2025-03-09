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

            if(value != GameEndings.None)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);



        }
    }

    private void Start()
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
}


public enum GameEndings
{
    Burning,
    Abandonment,
    // player death or child left alone for too long
    Monsters,
    Success,
    None
}
