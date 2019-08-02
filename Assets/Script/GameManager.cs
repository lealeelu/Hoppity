using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        gameOverPanel.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        startPanel.SetActive(true);
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
                    frog.JumpToLillyPad(hit.transform.gameObject.GetComponent<LillyPad>());
                    if (!_playing)
                    {
                        _playing = true;
                        map.StartMap();
                    }
                }
            }
        }
    }
    
    public void TryAgain()
    {
        hudPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        startPanel.SetActive(true);
    }
    
    public void StartGame()
    {
        map.GenerateMap();
        startPanel.SetActive(false);
        hudPanel.SetActive(true);
    }
    
}
