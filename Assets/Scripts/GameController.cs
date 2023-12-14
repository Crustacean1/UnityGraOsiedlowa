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

public class GameController : MonoBehaviour
{
    private int selectedCard = -1;
    private LevelDefinition levelInfo;
    private Player player;

    public int CardsToDraw;

    public LevelDefinition LevelInfo => levelInfo;
    public Player Player => player;

    public List<BuildingDefinition> BuildingDefinitions;
    public List<BuildingDefinition> CurrentDeck;

    public BuildingDefinition SelectedCard => selectedCard == -1 ? null : CurrentDeck[selectedCard];

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<GodScript>().SingleOrDefault() is GodScript godScript)
        {
            levelInfo = godScript.CurrentLevel;
            player = godScript.Player;
        }
        var buildings = Resources.Load<TextAsset>("BuildingDefinitions/buildings");
       // BuildingDefinitions = JsonUtility.FromJson<BuildingContainer>(buildings?.text ?? "[]")?.Buildings ?? new List<BuildingDefinition>();
    }

    void SelectCard(int id)
    {
        selectedCard = id;
    }

    void DrawCards()
    {
        List<int> newCards = new();
        System.Random random;

        while (newCards.Count() < CardsToDraw)
        {
          //  int pretender = random.Next(0, BuildingDefinitions.Count());
         //   if (!newCards.Contains(pretender)) { newCards.Add(pretender); }
        }
        CurrentDeck = newCards.Select(i => BuildingDefinitions[i]).ToList();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
