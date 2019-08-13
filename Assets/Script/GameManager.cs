﻿using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    public Frog frog;
    [SerializeField]
    private Map map;

    // UI
    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private GameObject hudPanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject SettingsPanel;
    [SerializeField]
    private GameObject StoryPanel;
    [SerializeField]
    private GameObject CreditsPanel;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI highScoreText;
    [SerializeField]
    private TextMeshProUGUI versionText;

    private float highScore = 0;
    private float currentScore = 0;

    public bool Playing
    {
        get
        {
            return _playing;
        }
    }

    private bool _playing = false;

    public void EndGame()
    {
        _playing = false;
        //save score to file
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save();
            highScoreText.text = scoreText.text;
        }        

        gameOverPanel.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        startPanel.SetActive(true);
        versionText.text = string.Format("Version {0}", Application.version);
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetFloat("HighScore");
            highScoreText.text = ((int)highScore).ToString("D10");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPanel.activeSelf && !gameOverPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "LillyPad")
                {
                    //jump to that lillypad
                    frog.JumpToLillyPad(hit.transform.gameObject.GetComponent<LillyPad>(), 2);
                    if (!_playing) StartGame();
                }
            }
        }
    }

    public void StartGame()
    {
        _playing = true;
        map.StartMap();
    }

    public void TryAgain()
    {
        SetBoard();
    }
    
    public void GoBackToStart()
    {
        hudPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        map.ClearMap();
        startPanel.SetActive(true);
        frog.gameObject.SetActive(false);
    }

    public void SetBoard()
    {
        frog.gameObject.SetActive(true);
        map.GenerateMap();
        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        hudPanel.SetActive(true);
    }

    public void UpdateScore(float newScore)
    {
        currentScore = newScore;
        scoreText.text = ((int)newScore).ToString("D10");
    }

    public void Settings(bool open)
    {
        SettingsPanel.SetActive(open);
        startPanel.SetActive(!open);
    }

    public void Story(bool open)
    {
        StoryPanel.SetActive(open);
        startPanel.SetActive(!open);
    }

    public void Credits(bool open)
    {
        CreditsPanel.SetActive(open);
        startPanel.SetActive(!open);
    }

    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
