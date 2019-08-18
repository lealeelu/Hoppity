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

    [Header("Settings")]
    [SerializeField]
    private int poolsize = 20;
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
    private float startingSpawnRate = 2f;
    [SerializeField]
    private float currentSpawnRate;
    [SerializeField]
    private float SRDecreasePerSec = 0.01f;

    [Header("Difficulty - mapspeed")]
    [SerializeField]
    private float currentMapSpeed = 1f;
    [SerializeField]
    private float startingMapSpeed;
    [SerializeField]
    private float MSIncreasePerSec = 0.1f;
    [SerializeField]
    private float MaxMapSpeed = 45f;

    private Pool lilyPool;
    private Pool flyPool;
    private Pool flowerPool;
    private float gameStart;
    private float spawnTimer;
    private float difficultyTimer;
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
    }

    void Update()
    {
        if (!GameManager.Instance.Playing) return;

        if (currentMapSpeed < MaxMapSpeed)
        {
            //up the difficulty every second
            difficultyTimer += Time.smoothDeltaTime;
            if (difficultyTimer > 1f)
            {
                currentMapSpeed += MSIncreasePerSec;
                currentSpawnRate -= SRDecreasePerSec;
                difficultyTimer = 0;
            }
        }

        spawnTimer += Time.smoothDeltaTime;
        if (spawnTimer > currentSpawnRate)
        {
            spawnTimer = 0;
            SpawnLillyRow();
        }
    }

    public void GenerateMap()
    {
        //Reset for new game
        currentMapSpeed = startingMapSpeed;
        currentSpawnRate = startingSpawnRate;
        lillyCount = 0;

        // disable all current lillypads and start over
        ClearMap();

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
        spawnTimer = currentSpawnRate;
        difficultyTimer = 0;
    }

    public void ClearMap()
    {
        lilyPool.DeactivatePoolObjects();
        flyPool.DeactivatePoolObjects();
        flowerPool.DeactivatePoolObjects();
    }

    void SpawnLillyRow(float startingZ = -1)
    {
        Transform[] rowTransforms;

        if (isRowA) rowTransforms = RowASpawns;
        else rowTransforms = RowBSpawns;

        //Here is where I decide what kind of lillies to spawn
        int countPerRow = Random.Range(1, 4);
        //if there is only one lilly, spawn it as one normal lilly
        int firstSpawnPos = Random.Range(0, 2);
        SpawnInRow(rowTransforms[firstSpawnPos], LillyPad.Type.Normal, startingZ);

        if (countPerRow > 1)
        {
            int secondSpawnPos = (firstSpawnPos + 1) % 3;
            SpawnInRow(rowTransforms[secondSpawnPos], LillyPad.Type.Fly, startingZ);
        }

        if (countPerRow > 2)
        {
            int thirdSpawnPos = (firstSpawnPos + 2) % 3;
            SpawnInRow(rowTransforms[thirdSpawnPos], LillyPad.Type.Flower, startingZ);
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
