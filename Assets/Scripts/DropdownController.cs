using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

/// <summary>
/// Represents information for a specific level.
/// </summary>
[Serializable]
public class LevelInfo
{
    public string Name;
    public int Id;
    public int BombCount;
    public Sprite Img;
    public string[] Parameters;
}

/// <summary>
/// Controls the dropdown menu for selecting levels.
/// </summary>
public class DropdownController : MonoBehaviour
{
    /// <summary>
    /// Array of level descriptions.
    /// </summary>
    public LevelInfo[] LevelDescription;

    /// <summary>
    /// The dropdown UI element to display levels.
    /// </summary>
    public TMP_Dropdown LevelDropdown;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var level in LevelDescription)
        {
            var option = new TMP_Dropdown.OptionData(level.Name, level.Img);
            LevelDropdown.options.Add(option);
            UnityEngine.Debug.Log($"Level {level.Name}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
