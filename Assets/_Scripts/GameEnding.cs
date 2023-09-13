using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class GameEnding : MonoBehaviour
{
    public float currentRunTime;
    public Transform barrier;
    public Transform player;
    public Text onScreenTimer;
    public GameObject timerObject;
    public GameObject endScreenObject;
    public GameObject inputField;

    bool m_gameWon;
    bool m_WasPlayerTouched;
    public float m_FinalEnemiesRemaining;
    bool m_MoveFinalBarrier;
    bool timing;
    int finalTime = 0;

    private void Start()
    {
        currentRunTime = 0;
        m_FinalEnemiesRemaining = 5;
        timing = true;
    }

    public void EnemyKilled()
    {
        m_FinalEnemiesRemaining -= 1;
    }

    public void TouchedPlayer()
    {
        m_WasPlayerTouched = true;
    }

    private void Update()
    {

        if (timing)
        {
            currentRunTime += Time.deltaTime;
            int seconds = Mathf.FloorToInt(currentRunTime);
            onScreenTimer.text = ("Current Run: " + seconds.ToString());
        }

        if (m_WasPlayerTouched)
        {
            EndLevel();
        }

        if(m_FinalEnemiesRemaining < 1)
        {
            MoveBarrier();
        }

        if (m_MoveFinalBarrier)
        {
            float barrierSpeed = 0.3f * Time.deltaTime;
            barrier.position = Vector2.MoveTowards(barrier.position, new Vector2(barrier.position.x, barrier.position.y + 1), barrierSpeed);
        }

        if (m_gameWon)
        {
            player.transform.position = new Vector2(10000, 0);
            string text = inputField.GetComponent<TMP_InputField>().text;
            if(Input.GetKeyDown(KeyCode.Return))
            {
                if (text.Length >= 3)
                {
                    finalTime = Mathf.FloorToInt(currentRunTime);
                    SaveTime(text, finalTime);
                    endScreenObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform == player)
        {
            CompleteLevel();
        }
    }

    void EndLevel()
    {
        //Runs when the player dies
        SceneManager.LoadScene("Level 1");
    }

    void MoveBarrier()
    {
        //Moves the pillar/gate blocking the way
        m_MoveFinalBarrier = true;
    }

    void CompleteLevel()
    {
        //Runs when a player actually finishes and reaches the cave exit
        timing = false;
        timerObject.SetActive(false);
        endScreenObject.SetActive(true);
        m_gameWon = true;
    }

    void SaveTime(string playerName, int runTime)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/highScores.dat");

        HighScoreData data = new HighScoreData();
        data.playerName = playerName;
        data.runTime = runTime;

        bf.Serialize(file, data);
        file.Close();
        SceneManager.LoadScene("Main Menu");
    }

}

[Serializable]
class HighScoreData
{
    public int runTime;
    public string playerName;
}
