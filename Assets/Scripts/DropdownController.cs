using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

[Serializable]
public class LevelInfo
{
    public string Name;
    public int Id;
    public int BombCount;
    public Sprite Img;
    public string[] Parameters;
}

public class DropdownController : MonoBehaviour
{
    public LevelInfo[] LevelDescription;

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
