using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "level", menuName = "Level")]
public class LevelScriptable : ScriptableObject
{
    public string sceneName;
    public Sprite spr;

    public void AddLevel()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}
