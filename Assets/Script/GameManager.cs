using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("Settings")]
    [SerializeField]
    public AnimationCurve difficultyCurve;
    [SerializeField]
    public float SuperModeLength = 6f;

    [Header("References")]
    [SerializeField]
    public Frog frog;
    [SerializeField]
    private Map map;

    [Header("UI")]
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
    [SerializeField]
    public FillBar flyCountBar;
    [SerializeField]
    private Button superModeButton;
    [SerializeField]
    private Animation superModeButtonAnimation;
    [SerializeField]
    private AddPointUI notificationUI;
    [SerializeField]
    public float MaxGameTime = 240f;

    private float highScore = 0;
    private float currentScore = 0;
    private static string gameID = "3254786";
    private static string placementID = "gameOverBanner";
    // Time since start of this playthrough
    public float gameTimer;
    private float _currentDifficulty;
    public bool SuperModeActive = false;
    private Camera camera;

    public float CurrentDifficulty
    {
        get
        {
            return _currentDifficulty;
        }
    }
    
    public bool Playing
    {
        get
        {
            return _playing;
        }
    }

    private bool _playing = false;    

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        startPanel.SetActive(true);
        versionText.text = string.Format("Version {0}", Application.version);
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetFloat("HighScore");
            highScoreText.text = ((int)highScore).ToString("D10");
        }
        Advertisement.Initialize(gameID, true);
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        flyCountBar.OnCountdownFinished = DeactivateSuperMode;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playing)
        {
            //update the difficulty based on a curve.
            gameTimer += Time.deltaTime;
            _currentDifficulty = difficultyCurve.Evaluate(gameTimer / MaxGameTime);
        }

        if (!startPanel.activeSelf && !gameOverPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "LillyPad")
                {
                    //jump to that lillypad
                    LillyPad lily = hit.transform.gameObject.GetComponent<LillyPad>();
                    frog.JumpToLillyPad(lily, 2);
                    if (!_playing) StartGame();
                }
            }
        }
    }

    public void StartGame()
    {
        _playing = true;
        map.StartMap();
        gameTimer = 0;
    }

    public void EndGame()
    {
        _playing = false;
        AudioManager.Instance.PlayBG(false);
        if(SuperModeActive)
        {
            DeactivateSuperMode();
        }
        //save score to file
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save();
            highScoreText.text = scoreText.text;
        }

        gameOverPanel.SetActive(true);
        StartCoroutine(ShowBannerWhenReady());
    }

    
    IEnumerator ShowBannerWhenReady()
    {
#if UNITY_ANDROID || UNITY_IOS
        while (!Advertisement.IsReady(placementID))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(placementID);
#else
        yield return null;
#endif
    }

    public void TryAgain()
    {
        Advertisement.Banner.Hide();
        SetBoard();
    }
    
    public void GoBackToStart()
    {
        hudPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        Advertisement.Banner.Hide();

        map.ClearMap();
        startPanel.SetActive(true);
        frog.gameObject.SetActive(false);
    }

    public void SetBoard()
    {
        gameTimer = 0.001f;
        _currentDifficulty = difficultyCurve.Evaluate(gameTimer / MaxGameTime);

        frog.gameObject.SetActive(true);
        map.GenerateMap();
        flyCountBar.SetValue(0);
        hudPanel.SetActive(true);

        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        AudioManager.Instance.PlayBG();
    }

    public void AddFirefly()
    {
        flyCountBar.Increment();
        if (flyCountBar.ReachedMax())
        {
            if (!superModeButton.enabled)
            {
                superModeButton.enabled = true;
                //start flashing animation on button so the player knows they can use it.
                superModeButtonAnimation.Play("ButtonPulse");
            }            
        }
    }

    public void ActivateSuperMode()
    {
        superModeButtonAnimation.Play("ButtonIdle");
        superModeButton.enabled = false;
        flyCountBar.CountDown(SuperModeLength);
        SuperModeActive = true;
        frog.ActivateSuperMode();
    }

    public void DeactivateSuperMode()
    {
        flyCountBar.StopCountdown();
        frog.StopSuperMode();
        SuperModeActive = false;
    }

    public void UpdateScore(float newScore)
    {
        currentScore = newScore;
        scoreText.text = ((int)newScore).ToString("D10");
    }

    public void ShowNotification(string text, Vector3 position)
    {
        Vector3 screenPos = camera.WorldToScreenPoint(position);
        notificationUI.ShowPoints(text, new Vector2(screenPos.x, screenPos.y));
    }

    public void ShowNotification(string text, Vector2 position)
    {
        notificationUI.ShowPoints(text, position);
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
