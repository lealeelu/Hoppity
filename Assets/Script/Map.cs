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
    private float startingSpawnRate = 2f;
    [SerializeField]
    private float currentSpawnRate;
    [SerializeField]
    private float difficultyMultiplier = 1.1f;

    [SerializeField]
    private float currentMapSpeed = 1f;
    [SerializeField]
    private float startingMapSpeed;
    [SerializeField]
    private int starterPads = 7;
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
        // add starter pads and activate
        currentMapSpeed = startingMapSpeed;
        currentSpawnRate = startingSpawnRate;
        for (int i = 0; i < lillyPadPool.Count; i++)
        {
            LillyPad pad = lillyPadPool[i];
            if (i < starterPads)
            {
                // set position
                float newZ = (TopLeft.transform.position.z - BottomRight.transform.position.z)/starterPads*i;
                pad.transform.position = new Vector3(GetRandX(), 0, newZ);
                pad.gameObject.SetActive(true);
                pad.enabled = false;
                pad.speed = currentMapSpeed;
                
                if (i == 0)
                {
                    // put frog on first lillypad
                    GameManager.Instance.frog.TeleportToLillyPad(pad);
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
        
        gameStart = Time.time;
        spawnTimer = currentSpawnRate;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadLillyPadPool();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.Playing) return;
        
        //up the difficulty
        //currentMapSpeed += (Time.deltaTime * difficultyMultiplier);
        //currentSpawnRate -= (Time.deltaTime * difficultyMultiplier);

        spawnTimer += Time.smoothDeltaTime;
        if (spawnTimer > currentSpawnRate)
        {
            //Debug.Log("spawn " + spawnTimer);
            spawnTimer = 0;
            StartCoroutine(SpawnLilly());
        }
    }

    IEnumerator SpawnLilly()
    {
        LillyPad pad = GetInactiveLillyPad();
        pad.gameObject.transform.position = new Vector3(GetRandX(), 0, TopLeft.transform.position.z);
        pad.speed = currentMapSpeed;
        pad.gameObject.SetActive(true);
        yield return null;
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
