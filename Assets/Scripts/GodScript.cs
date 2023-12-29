using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.Json;
using UnityEngine.SceneManagement;

/// <summary>
/// Represents a requirement in the game.
/// </summary>
[Serializable]
public class Requirement
{
    public string Name;
    public float Current;
    public float Min;
    public float Max;
}

/// <summary>
/// Represents the definition of a game level.
/// </summary>
[Serializable]
public class LevelDefinition
{
    public int Bombs;
    public List<Requirement> Requirements;
    public string Name;
    public string Difficulty;
}

/// <summary>
/// Represents a property of a building.
/// </summary>
[Serializable]
public class BuildingProperty
{
    public string Name;
    public string Value;
}

/// <summary>
/// Represents the definition of a building.
/// </summary>
[Serializable]
public class BuildingDefinition
{
    public string Name;
    public string Sprite;
    public string Mesh;

    public Dictionary<string, float> Properties = new Dictionary<string, float>();
}

/// <summary>
/// Represents a player in the game.
/// </summary>
public class Player
{
    public string Name;
}

/// <summary>
/// Controls the game state and transitions between scenes.
/// </summary>
public class GodScript : MonoBehaviour
{
    Player player;

    LevelDefinition currentLevel;

    public List<LevelDefinition> LevelDefinitions;
    public List<BuildingDefinition> BuildingDefinitions;

    /// <summary>
    /// Accessor for the current level definition.
    /// </summary>
    public LevelDefinition CurrentLevel => currentLevel;

    /// <summary>
    /// Accessor for the player information.
    /// </summary>
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

    /// <summary>
    /// Starts the game with the selected player and level.
    /// </summary>
    /// <param name="player">The player to start the game with.</param>
    /// <param name="selectedLevel">The index of the selected level.</param>
    public void StartGame(Player player, int selectedLevel)
    {
        currentLevel = LevelDefinitions[selectedLevel];
        this.player = player;

        SceneManager.LoadSceneAsync("MainGameScene", LoadSceneMode.Single);
    }

    /// <summary>
    /// Finishes the current game and loads the end scene.
    /// </summary>
    public void FinishGame()
    {
        SceneManager.LoadSceneAsync("EndScene", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
