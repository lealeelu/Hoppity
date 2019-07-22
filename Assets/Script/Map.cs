using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    private GameObject lillyPadPrefab;
    [SerializeField]
    private int poolsize = 20;
    [SerializeField]
    private float spawnRate = 2f;
    [SerializeField]
    private float startDistance = 2f;

    [SerializeField]
    private float mapSpeed = 1f;
    [SerializeField]
    private Transform TopLeft;
    [SerializeField]
    private Transform BottomRight;

    private List<LillyPad> lillyPadPool;
    private float gameStart;
    private float spawnTimer;
    
    public void GenerateMap()
    {
        // disable all current lillypads
        // add 6 starter pads and activate
        int starterPads = 6;
        for (int i = 0; i < lillyPadPool.Count; i++)
        {
            LillyPad pad = lillyPadPool[i];
            if (i < starterPads)
            {
                // set position
                float newZ = (TopLeft.transform.position.z - BottomRight.transform.position.z)/6*i;
                pad.transform.position = new Vector3(GetRandX(), 0, newZ);
                pad.gameObject.SetActive(true);
                pad.enabled = false;
                pad.speed = mapSpeed;
                
                if (i == 0)
                {
                    // put frog on first lillypad
                    GameManager.Instance.frog.JumpToLillyPad(pad);
                }
            }
            else pad.gameObject.SetActive(false);
        }
    }

    public void StartMap()
    {
        //enable the pads already spawned
        for (int i = 0; i < lillyPadPool.Count; i++)
        {
            if (lillyPadPool[i].gameObject.activeSelf) lillyPadPool[i].enabled = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadLillyPadPool();
        gameStart = Time.time;
        spawnTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.Playing) return;
        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnRate)
        {
            LillyPad pad = GetInactiveLillyPad();
            pad.transform.position = new Vector3(GetRandX(), 0, TopLeft.transform.position.z);
            pad.speed = mapSpeed;
            pad.gameObject.SetActive(true);
            spawnTimer = 0;
        }
    }

    void LoadLillyPadPool()
    {
        lillyPadPool = new List<LillyPad>();
        for (int i = 0; i < poolsize; i++)
        {
            lillyPadPool.Add(GenerateLillyPad());
        }
    }

    LillyPad GetInactiveLillyPad()
    {
        for (int i = 0; i < lillyPadPool.Count; i++)
        {
            if (!lillyPadPool[i].isActiveAndEnabled) return lillyPadPool[i];
        }
        //if we've gotten here, they are all being used. EXPAND!!
        LillyPad pad = GenerateLillyPad();
        lillyPadPool.Add(pad);
        return pad;
    }

    LillyPad GenerateLillyPad()
    {
        GameObject pad = GameObject.Instantiate(lillyPadPrefab);
        pad.SetActive(false);
        return pad.GetComponent<LillyPad>();
    }

    float GetRandX()
    {
        return Random.Range(TopLeft.position.x, BottomRight.position.x);
    }
}
