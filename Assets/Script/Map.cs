using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject normalLilyPadPrefab;
    [SerializeField]
    private GameObject flowerLilyPadPrefab;
    [SerializeField]
    private GameObject flyLilyPadPrefab;
    [SerializeField]
    private GameObject sinkLilyPadPrefab;

    [Header("Settings")]
    [SerializeField]
    private int starterPads = 6;
    [SerializeField]
    private Transform TopLeft;
    [SerializeField]
    private Transform BottomRight;
    [SerializeField]
    private Transform startingPosition;

    [SerializeField]
    private Transform[] RowASpawns;
    [SerializeField]
    private Transform[] RowBSpawns;

    [Header("Difficulty - spawnrate")]
    [SerializeField]
    private float maxSpawnRate = 2f;

    [Header("Difficulty - mapspeed")]
    [SerializeField]
    private float currentMapSpeed = 1f;
    [SerializeField]
    private float maxMapSpeed = 10f;
            
    private Pool lilyPool;
    private Pool flyPool;
    private Pool flowerPool;
    private Pool sinkPool;
    private float gameStart;
    private float spawnTimer;
    private float difficultyTimer;
    private float gameTimer;
    private int lillyCount = 0;
    private bool isRowA = true;

    void Start()
    {
        lilyPool = new Pool(normalLilyPadPrefab);
        lilyPool.LoadPool(20);

        flyPool = new Pool(flyLilyPadPrefab);
        flyPool.LoadPool(15);

        flowerPool = new Pool(flowerLilyPadPrefab);
        flowerPool.LoadPool(15);

        sinkPool = new Pool(sinkLilyPadPrefab);
        sinkPool.LoadPool(15);
    }

    void Update()
    {
        if (!GameManager.Instance.Playing) return;
        currentMapSpeed = (maxMapSpeed * GameManager.Instance.CurrentDifficulty);
        spawnTimer += Time.deltaTime;
        if (spawnTimer > maxSpawnRate * (1 - GameManager.Instance.CurrentDifficulty))
        {
            spawnTimer = 0;
            SpawnLillyRow();
        }
    }

    public void GenerateMap()
    {
        //Reset for new game
        lillyCount = 0;

        // disable all current lillypads and start over
        ClearMap();
        currentMapSpeed = (maxMapSpeed * GameManager.Instance.CurrentDifficulty);

        // add starter rows
        for (int i = 0; i < starterPads; i++)
        {
            if (i == 0)
            {
                // put frog on first lillypad
                LillyPad pad = SpawnLilly(startingPosition.position, LillyPad.Type.Normal);
                lillyCount++;
                GameManager.Instance.frog.TeleportToLillyPad(pad);
            }
            else
            {
                // set position
                float newZ = (TopLeft.transform.position.z - BottomRight.transform.position.z) / starterPads * i;
                SpawnLillyRow(newZ);
            }
        }
    }

    public void StartMap()
    {
        gameStart = Time.time;
        spawnTimer = maxSpawnRate;
        difficultyTimer = 0;
        gameTimer = 0;
    }

    public void ClearMap()
    {
        lilyPool.DeactivatePoolObjects();
        flyPool.DeactivatePoolObjects();
        flowerPool.DeactivatePoolObjects();
        sinkPool.DeactivatePoolObjects();
    }

    void SpawnLillyRow(float startingZ = -1)
    {
        Transform[] rowTransforms;

        if (isRowA) rowTransforms = RowASpawns;
        else rowTransforms = RowBSpawns;

        //Here is where I decide what kind of lillies to spawn
        //int countPerRow = Random.Range(1, 4);
        //if there is only one lilly, spawn it as one normal lilly
        int firstSpawnPos = Random.Range(0, 2);
        SpawnInRow(rowTransforms[firstSpawnPos], LillyPad.Type.Normal, startingZ);

        if (Random.value > 0.5)
        {
            int secondSpawnPos = (firstSpawnPos + 1) % 3;
            SpawnInRow(rowTransforms[secondSpawnPos], LillyPad.GetRandomType(), startingZ);
        }

        if (Random.value > 0.5)
        {
            int thirdSpawnPos = (firstSpawnPos + 2) % 3;
            SpawnInRow(rowTransforms[thirdSpawnPos], LillyPad.GetRandomType(), startingZ);
        }

        isRowA = !isRowA;
        lillyCount++;
    }

    void SpawnInRow(Transform t, LillyPad.Type type, float startingZ = -1)
    {
        Vector3 targetPos = t.position;
        if (startingZ != -1) targetPos = new Vector3(t.position.x, t.position.y, startingZ);
        SpawnLilly(targetPos, type);
    }

    LillyPad SpawnLilly(Vector3 position, LillyPad.Type type)
    {
        GameObject o = null;
        LillyPad pad;
        switch (type)
        {
            case LillyPad.Type.Normal:
                o = lilyPool.GetInactiveObject();
                break;
            case LillyPad.Type.Fly:
                o = flyPool.GetInactiveObject();
                break;
            case LillyPad.Type.Flower:
                o = flowerPool.GetInactiveObject();
                break;
            case LillyPad.Type.Sink:
                o = sinkPool.GetInactiveObject();
                break;
        }
        if (o != null)
        {
            o.transform.position = position;
            o.SetActive(true);
            pad = o.GetComponent<LillyPad>();
            pad.speed = currentMapSpeed;
            pad.lillyNumber = lillyCount;
            return pad;
        }
        else
        {
            Debug.LogError("Spawned Lily has no lily component");
            return null;
        }
    }
        
}
