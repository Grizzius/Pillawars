using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; set; }
    public List<Vector3> spawnPosition;
    public List<Vector3> spawnPositionAvailable;
    public List<Color> playerColors;
    List<Color> playerColorsAvailable;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        playerColorsAvailable = new();
        playerColorsAvailable = playerColors;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSpawn( Vector3 pos)
    {
        spawnPosition.Add(pos);
        spawnPositionAvailable = spawnPosition;
    }

    public void ClearSpawnList()
    {
        spawnPosition = new List<Vector3>();
    }

    public void RollSpawn(Transform playerTransform)
    {
        Debug.Log(spawnPositionAvailable.Count);
        var rand = Random.Range(0, spawnPositionAvailable.Count);
        var pos = spawnPositionAvailable[rand];
        spawnPositionAvailable.RemoveAt(rand);
        playerTransform.position = pos;

        //AssignColor(playerTransform.GetComponent<PlayerFX>()) ;
    }

    public void AssignColor(PlayerFX playerFX)
    {
        var rand = Random.Range(0, playerColorsAvailable.Count);
        var color = playerColorsAvailable[rand];
        playerColorsAvailable.RemoveAt(rand);
        playerFX.InitColor(color);
    }

    public void FoundSpawner()
    {
        ClearSpawnList();
        var parentSpawn = GameObject.Find("/Spawns");

        foreach (Transform spawn in parentSpawn.GetComponentInChildren<Transform>()) 
        {
            Debug.Log(spawn.name);
            AddSpawn(spawn.position);
        }
    }
}
