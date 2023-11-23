using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[Serializable]
public class LevelDefinition
{
    public int Bombs;
    public Parameter Parameters;
    public string Name;
    public string Difficulty;
}

public class MainMenu : MonoBehaviour
{
    public int difficulty;
    public int Difficulty => difficulty;

    public TMP_Text Username;

    public LevelDefinition[] LevelDefinitions;

    public Transform LevelPanel;
    public GameObject LevelLabelPrefab;

    // Start is called before the first frame update
    void Start()
    {
        CreateLevels();
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void CreateLevels()
    {
        int i = 0;
        float gap = 40;
        float start = (LevelDefinitions.Length -1)* gap * 0.5f;
        Func<int,Vector3> getPosition = (int i) => { return (new Vector3(0f, start - i * gap, 0f)); } ;

        foreach(var definition in LevelDefinitions)
        {
            var level = Instantiate(LevelLabelPrefab);
            level.transform.SetParent(LevelPanel);
            var rectTransform = level.GetComponent<RectTransform>();
            level.GetComponent<ButtonHover>()?.Instantiate(i, definition.Name, this);
            rectTransform.anchoredPosition = getPosition(i);
            i++;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDifficulty(int d) { difficulty = d; }

    public void StartGame()
    {
        if (Username.text.Length < 2)
        {
            //DisplayDialog("Cannot start the game", "No username selected", "Ok");
            return;
        }
        if(Difficulty == 0)
        {
            //DisplayDialog("Cannot start the game", "No difficulty selected", "Ok");
            return;
        }
        SceneManager.LoadSceneAsync("MainGameScene", LoadSceneMode.Single);
    }
}
