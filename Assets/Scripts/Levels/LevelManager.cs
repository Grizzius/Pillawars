using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<LevelScriptable> levels;

    private void Awake()
    {
        levels[0].AddLevel();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnManager.Instance.FoundSpawner();
    }

    // Update is called once per frame
    void Update()
    {
            
    }
}
