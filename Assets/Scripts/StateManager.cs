using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.IO;

public class StateManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public static StateManager Instance;
    public string highScoreName;
    public int highScoreScore;
    public string playerName;
    public TMP_InputField inputField;   
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
            return;
        }
        Instance= this;
        DontDestroyOnLoad(gameObject);  
    }

    private void Start()
    {
        if (LoadHighScore())
        {
            highScoreText.text = "Best Score: " + highScoreName + ": " + highScoreScore;
        } else
        {
            highScoreText.text = "Best Score: ";
        }

    }

    public void loadMainScene()
    {
        playerName = inputField.text;

        Debug.Log("playerName = " + playerName);
        if(playerName != "") { SceneManager.LoadScene(1); }       
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }


    [System.Serializable]

    class HighScore
    {
        public string name;
        public int Score;
    }

    public void SaveHighScore()
    {
        HighScore highScore = new HighScore();
        highScore.name = highScoreName;
        highScore.Score = highScoreScore;

        String json = JsonUtility.ToJson(highScore);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

        Debug.Log("Saving: " + highScoreName + ": " + highScoreScore);
    }

    public bool LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);   
            HighScore highScore = JsonUtility.FromJson<HighScore>(json);

            highScoreName = highScore.name;
            highScoreScore = highScore.Score;

            Debug.Log("Loading and retrieved: " + highScoreName + ": " + highScoreScore);

            return true;
        }
        return false;
    }

    public void DeleteAllHighScores()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            //String json = JsonUtility.ToJson(null);
            //File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
            File.Delete(path);
            highScoreScore = 0;
            highScoreName = null;
            highScoreText.text = "Best Score: ";
        }
    }

}
