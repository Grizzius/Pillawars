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

    /// <summary>
    /// Retourne le nom de l'objet Racine de la scène à supprimer par la suite pour changer de niveau
    /// </summary>
    /// <returns></returns>
    public string AddLevel()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        return sceneName;
    }
}
