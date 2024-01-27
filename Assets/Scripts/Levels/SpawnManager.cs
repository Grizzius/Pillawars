using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; set; }
    public List<Vector3> spawnPosition;
    public List<Vector3> spawnRotationAvailable;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSpawn( Vector3 pos)
    {
        spawnPosition.Add(pos);
        spawnRotationAvailable = spawnPosition;
    }

    public void ClearSpawnList()
    {
        spawnPosition = new List<Vector3>();
    }

    public Vector3 RollSpawn()
    {
        var rand = Random.Range(0, spawnRotationAvailable.Count);
        var pos = spawnRotationAvailable[rand];
        spawnRotationAvailable[rand] = pos;
        spawnRotationAvailable.RemoveAt(rand);
        return pos;
    }

    public void FoundSpawner()
    {
        ClearSpawnList();
        var parentSpawn = GameObject.Find("/Spawns");

        foreach (Transform spawn in parentSpawn.GetComponentInChildren<Transform>()) 
        {
            AddSpawn(spawn.position);
        }
    }
}
