using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPivot : MonoBehaviour
{
    List<PlayerInput> playerList;
    Camera camera;
    public float camSpeed;
    public float zoomSpeed;
    public float minDistance;

    enum Mode
    {
        game,
        podium
    }

    // Start is called before the first frame update
    void Start()
    {
        playerList = new();
        camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerList.Count > 0)
        {
            Vector2 targetPosition = PlayersAveragePosition();
            float xPosition = Mathf.SmoothStep(transform.position.x, targetPosition.x, Time.deltaTime / camSpeed);
            float yPosition = Mathf.SmoothStep(transform.position.z, targetPosition.y, Time.deltaTime / camSpeed);
            transform.position = new Vector3(xPosition, 0, yPosition);

            float zoom = Mathf.SmoothStep(camera.transform.position.y, CalculateCamHeight(), Time.deltaTime / zoomSpeed);
            camera.transform.localPosition = new Vector3(0, zoom, -2);
        }
    }

    public void AddPlayer(PlayerInput player)
    {
        playerList.Add(player);
    }

    Vector2 PlayersAveragePosition()
    {
        float x = 0f;
        float y = 0f;
        foreach(PlayerInput player in playerList)
        {
            x += player.transform.position.x;
            y += player.transform.position.z;
        }
        x = x / playerList.Count;
        y = y / playerList.Count;
        return new Vector2(x, y);
    }

    float CalculateCamHeight()
    {
        float[] xArray = new float[playerList.Count];
        float[] yArray = new float[playerList.Count];

        for(int i = 0; i < playerList.Count; i++)
        {
            xArray[i] = playerList[i].transform.position.x;
            yArray[i] = playerList[i].transform.position.z;
        }

        float distanceX = xArray.Max() - xArray.Min();
        float distanceY = yArray.Max() - yArray.Min();

        return Mathf.Max(distanceX, distanceY) + minDistance;
    }

    public void Reset()
    {
        playerList.Clear();
    }

    public void RemovePlayer(PlayerInput player)
    {
        if(playerList.Contains(player))
        {
            playerList.Remove(player);
        }
    }

    /// <summary>
    /// Reprends le focus sur l'ensemble des participants
    /// </summary>
    public void RetrieveAllPlayers()
    {
        playerList = new();
        playerList.AddRange(GameManager.Instance.playerInputList);
    }

    public bool IsStillOneGuy()
    {
        return playerList.Count == 1;
    }

    /// <summary>
    /// Renvoie le premier de la liste, sert notemment pour récupérer le vainqueur d'une partie
    /// </summary>
    /// <returns></returns>
    public Transform GetFirstPlayer()
    {
        if (playerList.Count > 0)
        {
            return playerList[0].transform;
        }
        else 
        { 
            return null;
        }
    }
}
