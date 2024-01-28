using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerJoinMenu : MonoBehaviour
{
    public DisplayPlayerJoined[] displaysPlayerJoined;
    public TextMeshProUGUI countDownMesh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCountDown(int countdown)
    {
        countDownMesh.text = "Game starts in " + countdown + "s";
    }
}
