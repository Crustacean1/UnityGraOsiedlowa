using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.Json;
using UnityEngine.SceneManagement;

[Serializable]
public class LevelDefinition
{
    public int Bombs;
    public Parameter Parameters;
    public string Name;
    public string Difficulty;
}

[Serializable]
public class BuildingProperty
{
    public string Name;
    public string Value;
}

[Serializable]
public class BuildingDefinition
{
    public string Name;
    public string Sprite;
    public string Mesh;

    List<BuildingProperty> Properties;
}

public class Player
{
    public string Name;
}

public class GodScript : MonoBehaviour
{
    Player player;
    LevelDefinition currentLevel;

    public List<LevelDefinition> LevelDefinitions;
    public List<BuildingDefinition> BuildingDefinitions;

    public LevelDefinition CurrentLevel => currentLevel;
    public Player Player => player;

    enum GameState
    {
        MainMenu,
        GamePlay,
        GamePause,
    }

    class BuildingContainer
    {
        public List<BuildingDefinition> Buildings;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void StartGame(Player player, int selectedLevel)
    {
        currentLevel = LevelDefinitions[selectedLevel];
        this.player = player;

        SceneManager.LoadSceneAsync("MainGameScene", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {

    }
}