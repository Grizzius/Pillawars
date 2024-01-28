using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; set; }
    public List<LevelScriptable> levels;
    public Transform bacALoser;

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
        t.position = bacALoser.position;
        GameManager.Instance.cameraPivot.RemovePlayer(t.GetComponent<PlayerInput>());
    }

    public void GoWaitingRoom()
    {

    }

    public void LoadRandomLevel()
    {
        int rand = Random.Range(0, levels.Count);
        levels[0].AddLevel();
        StartCoroutine(DelaySpawn());
    }

    /// <summary>
    /// petit décalage temporel pour s'assurer que le décor apparaisse avant que les joueurs arrivent sur le niveau 
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
    }
}
