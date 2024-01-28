using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; set; }
    public List<LevelScriptable> levels;
    public string nameCurrentScene;
    public Transform bacALoser;
    public Transform podiumLose;
    public Transform podiumWin;
    public ParticleSystem deathEffect;
    
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
        CheckWinner();
    }

    public void GoWinRoom(Transform transformWinner)
    {
        Debug.Log("GO WIN ROOM");
        foreach (PlayerInput p in GameManager.Instance.playerInputList)
        {
            if(p.transform !=  transformWinner)
            {
                p.transform.position = podiumLose.position;
            }
            else
            {
                p.transform.position = podiumWin.position;
            }
        }
    }

    public void LoadRandomLevel()
    {
        int rand = Random.Range(0, levels.Count);
        nameCurrentScene = levels[0].AddLevel();
        StartCoroutine(DelaySpawn());
    }

    /// <summary>
    /// petit d�calage temporel pour s'assurer que le d�cor apparaisse avant que les joueurs arrivent sur le niveau 
    /// </summary>
    /// <returns></returns>
    IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(0.5f);
        SpawnManager.Instance.FoundSpawner();
        foreach (var player in GameManager.Instance.playerInputList)
        {
            SpawnManager.Instance.RollSpawn(player.transform);
        }
        GameManager.Instance.cameraPivot.RetrieveAllPlayers();
    }

    public void CheckWinner()
    {
        Debug.Log("Win");
        if (GameManager.Instance.cameraPivot.IsStillOneGuy())
        {
            //T�l�portation podium
            StartCoroutine(NextLevel(GameManager.Instance.playerInputList[0].transform));
        }
    }

    IEnumerator NextLevel(Transform t)
    {
        Debug.Log("NEXT LEVEL");
        yield return new WaitForSeconds(2);
        GoWinRoom(t);
        yield return new WaitForSeconds(1);
        SceneManager.UnloadScene(nameCurrentScene);
        yield return new WaitForSeconds(5);
        Debug.Log(nameCurrentScene);
        LoadRandomLevel();
    }
}
