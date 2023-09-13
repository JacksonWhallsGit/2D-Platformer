using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseScreen;
    bool m_Paused = false;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !m_Paused)
        {
            PauseLevel();
        } else if (Input.GetKeyDown(KeyCode.Escape) && m_Paused)
        {
            ResumeLevel();
        }
    }

    public void MainMenuButton()
    {
        ResumeLevel();
        SceneManager.LoadScene("Main Menu");
    }

    public void RestartButton()
    {
        ResumeLevel();
        SceneManager.LoadScene("Level 1");
    }

    void PauseLevel()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
        m_Paused = true;
    }

    void ResumeLevel()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
        m_Paused = false;
    }
}
