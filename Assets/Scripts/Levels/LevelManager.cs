using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; set; }
    public List<LevelScriptable> levels;
    public Transform bacALoser;
    public ParticleSystem deathEffect;

    private void Awake()
    {
        Instance = this;
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

    public void PlayerLose(Transform t)
    {
        Instantiate(deathEffect, t.position, Quaternion.identity);
        t.position = bacALoser.position;
        GameManager.Instance.cameraPivot.RemovePlayer(t.GetComponent<PlayerInput>());
    }
}
