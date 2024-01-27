using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public string sceneMenuName;
    public PlayerJoinMenu playerJoinMenu;
    // Start is called before the first frame update
    void Start()
    {
        playerJoinMenu = GetComponentInChildren<PlayerJoinMenu>();
        ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (pauseMenu.activeSelf)
            {
                case true:
                    ResumeGame();
                    break;
                case false:
                    PauseGame();
                    break;
            }
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(sceneMenuName);
    }

    public void PlayerJoin(int playerID)
    {
        playerJoinMenu.displaysPlayerJoined[playerID].OnPlayerFound();
    }

    public void PlayerReady(int playerID)
    {
        playerJoinMenu.displaysPlayerJoined[playerID].OnPlayerReady();
    }
}
