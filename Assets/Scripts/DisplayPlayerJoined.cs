using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayPlayerJoined : MonoBehaviour
{
    TextMeshProUGUI text;
    Image image;
    public int playerID;
    public Color joinedColor;
    public Color readyColor;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = new string("Waiting player " + playerID + "...");
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerFound()
    {
        image.color = joinedColor;
        text.text = new string("Player " + playerID + " joined !");
    }

    public void OnPlayerReady()
    {
        image.color = readyColor;
        text.text = new string("Player " + playerID + " is ready !");
    }
}
