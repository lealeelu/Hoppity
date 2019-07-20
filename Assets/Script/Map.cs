using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int poolsize = 20;
    public GameObject lillyPadPrefab;
    public float spawnRate = 2f;
    public Transform spawnObject;
    public Transform deadObject;
    [SerializeField]
    private float mapSpeed = 1f;

    private List<LillyPad> lillyPadPool;
    private float gameStart;
    private float spawnTimer;

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
        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnRate)
        {
            LillyPad pad = GetInactiveLillyPad();
            pad.transform.position = spawnObject.transform.position;
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
        pad.transform.position = spawnObject.transform.position;
        pad.SetActive(false);
        return pad.GetComponent<LillyPad>();
    }
}
