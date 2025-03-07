using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject _pauseMenuGameObject;
    private bool _isPaused;
    [SerializeField] private bool _isMainMenu;

    public void Start()
    {
        if (!_isMainMenu)
            Unpause();
        else
        {
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isMainMenu)
        {
            if (_isPaused)
            {
                Unpause();
                Debug.Log("UnPausing");

            }
            else
            {
                Pause();
                Debug.Log("Pausing");
            }
        }

    }
    public void QuitGame()
    {
        Debug.Log("Quitting1");

        Application.Quit();
    }

    public void Pause()
    {
        Debug.Log("Pausing1");

        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
        if (_pauseMenuGameObject != null)
            _pauseMenuGameObject.SetActive(true);
        _isPaused = true;
    }

    public void Unpause()
    {
        Debug.Log("UnPausing1");

        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        if(_pauseMenuGameObject != null)
            _pauseMenuGameObject.SetActive(false);
        _isPaused = false;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
