using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class Leaderboards : MonoBehaviour
{
    public Transform scoreEntryExample;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    public int score;
    public string name;

    void Awake()
    {
        LoadFromFile();
        float entryExampleHeight = scoreEntryExample.GetComponent<RectTransform>().sizeDelta.y;

        for(int i=0; i<5; i++)
        {
            float newScore = UnityEngine.Random.Range(0, 1000);
            string scoreString = score.ToString();
            nameText.text = name;
            scoreText.text = scoreString;
            Transform scoreEntry = Instantiate(scoreEntryExample, gameObject.transform);
            RectTransform scoreEntryRectTransform = scoreEntry.GetComponent<RectTransform>();
            if (i == 0)
            {
                scoreEntryRectTransform.anchoredPosition = new Vector2(0, 310f);
            }
            else
            {
                scoreEntryRectTransform.anchoredPosition = new Vector2(0, -entryExampleHeight * i + 310);
            }
            scoreEntry.gameObject.SetActive(true);

        }

        scoreEntryExample.gameObject.SetActive(false);

    }

    public void LoadFromFile()
    {
        if (File.Exists(Application.persistentDataPath + "/highScores.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/highScores.dat", FileMode.Open);
            HighScoreData data = (HighScoreData)bf.Deserialize(file);

            score = data.runTime;
            name = data.playerName;

            Debug.Log(name + " and " + score);
        } else
        {
            score = 99999;
            name = "AAA";
        }
    }
}
