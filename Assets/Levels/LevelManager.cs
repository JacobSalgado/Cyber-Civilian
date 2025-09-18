using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    string[] level_list;
    GameObject current_level;

    Dictionary<string, string> level_dictionary = new Dictionary<string, string>
    {
        {"TestLevel", "Assets/Levels/TestLevel.prefab"}
    };

    private void LoadLevel(string level_name)
    {
        current_level = PrefabUtility.LoadPrefabContents(level_dictionary[level_name]);
        current_level.transform.SetParent(this.transform, false);
    }
    
    void Start()
    {
        LoadLevel(level_list[0]);
    }
}
