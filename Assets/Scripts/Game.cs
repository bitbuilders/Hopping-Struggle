using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : Singleton<Game>
{
    [SerializeField] GameObject m_pauseMenu = null;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (m_pauseMenu.activeInHierarchy)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        m_pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void UnPause()
    {
        m_pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
