using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInstance
{
    public Sprite sprite;
    public Mesh mesh;
}

public struct BuildingContainer
{
    public List<BuildingDefinition> Buildings;
}

public struct Score
{
    public float QualityOfLife;
    public int Population;
    public float Pkb;
}

public class GameController : MonoBehaviour
{
    private LevelDefinition levelInfo;
    private Player player;

    public int CardsToDraw;

    public LevelDefinition LevelInfo => levelInfo;
    public Player Player => player;

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
