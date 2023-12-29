using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an instance of a building with associated sprite and mesh.
/// </summary>
public class BuildingInstance
{
    public Sprite sprite;
    public Mesh mesh;
}

/// <summary>
/// Container structure for a list of building definitions.
/// </summary>
public struct BuildingContainer
{
    public List<BuildingDefinition> Buildings;
}

/// <summary>
/// Structure to store score-related information.
/// </summary>
public struct Score
{
    public float QualityOfLife;
    public int Population;
    public float Pkb;
}

/// <summary>
/// Controls the game logic and interactions.
/// </summary>
public class GameController : MonoBehaviour
{
    private LevelDefinition levelInfo;
    private Player player;

    /// <summary>
    /// Number of cards to draw in the game.
    /// </summary>
    public int CardsToDraw;

    /// <summary>
    /// Accessor for the current level information.
    /// </summary>
    public LevelDefinition LevelInfo => levelInfo;

    /// <summary>
    /// Accessor for the player information.
    /// </summary>
    public Player Player => player;

    /// <summary>
    /// Reference to the deck of cards.
    /// </summary>
    public Deck CardDeck;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<GodScript>().SingleOrDefault() is GodScript godScript)
        {
            levelInfo = godScript.CurrentLevel;
            player = godScript.Player;
        }
        else
        {
            levelInfo = new LevelDefinition
            {
                Bombs = 3,
                Requirements = new List<Requirement> {
                    new Requirement {
                    Name = "Population" ,
                    Min = 10,
                    Max = 100
                } ,
                    new Requirement {
                    Name = "GDP" ,
                    Min = 500,
                    Max = 1000
                }
                },
                Name = "Randy",
                Difficulty = "Dev"
            };
        }
    }

    /// <summary>
    /// Attempts to spend a bomb in the game.
    /// </summary>
    /// <returns>True if the bomb was successfully spent, false otherwise.</returns>
    public bool TrySpendBomb()
    {
        if (levelInfo.Bombs > 0)
        {
            levelInfo.Bombs -= 1;
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
