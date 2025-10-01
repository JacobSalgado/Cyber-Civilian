using System;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [NonSerialized] public Level current_level;

    public void LoadLevel(string level_name)
    {
        string level_path = string.Format("Assets/Levels/LevelList/{0}.prefab", level_name);

        current_level = PrefabUtility.LoadPrefabContents(level_path).GetComponent<Level>();
        if (current_level)
            current_level.transform.SetParent(this.transform, false);
        else
        {
            Debug.LogError(string.Format("Level: {0} doesn't exist in LevelList Folder", level_name));
        }
    }
}
