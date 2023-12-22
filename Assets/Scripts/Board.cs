using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;


public class BombDetonated
{
}

public class BuildingCreatedEvent
{
}

public enum PlayerAction
{
    Building,
    Bombing,
    Info
}

public class Board : MonoBehaviour
{
    private System.Random random = new System.Random();

    float timeSinceLastPedestrian = 0;

    public float PedestrianGeneration;
    public GameObject Pedestrian;
    public GameObject Fountain;
    public GameObject[] Trees;

    public GameController gameManager;
    public LevelInfoHud levelInfo;
    public GameObject Building;
    public BuildingHud BuildingHud;
    public int BoardSize;
    public GameObject Tile;
    public float Size;
    public NavMeshSurface navMesh;

    public BuildingDefinition SelectedBuildingDefinition { get; set; }
    public PlayerAction CurrentPlayerAction { get; set; }

    public event System.EventHandler<BombDetonated> BombDetonated;
    public event System.EventHandler<BuildingCreatedEvent> BuildingCreated;
    public bool RaycastEnabled { get; set; }

    void UpdateRequirements()
    {
        foreach (var requirement in gameManager.LevelInfo.Requirements)
        {
            requirement.Current = 0;
        }

        foreach (Transform tile_transform in transform)
        {
            if (tile_transform.GetComponent<Tile>() is Tile tile)
            {
                if (tile.GetBuildingDefinition() is BuildingDefinition definition)
                {
                    foreach (var property in definition.Properties)
                    {
                        UnityEngine.Debug.Log($"Found property: {property.Key}");
                        if (gameManager.LevelInfo.Requirements.SingleOrDefault(req => req.Name == property.Key) is Requirement value)
                        {
                            value.Current += property.Value;
                        }
                    }
                }
            }
        }
        levelInfo.Refresh();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < BoardSize * 2 - 1; i++)
        {
            Vector3 rowStart = new Vector3(0.5f * Mathf.Abs((float)i - BoardSize + 1), 0, 0.5f * i * Mathf.Sqrt(3)) * Size - (BoardSize - 1) * new Vector3(Size, 0, Size * 0.5f * Mathf.Sqrt(3));
            for (int j = 0; j < BoardSize + BoardSize - 1 - Mathf.Abs(i - BoardSize + 1); j++)
            {
                var tile = Instantiate(Tile, rowStart + j * new Vector3(Size, 0, 0), Quaternion.identity);
                tile.transform.parent = gameObject.transform;
                tile.transform.localRotation = Quaternion.Euler(Vector3.up * 30);

                Tile tile_component = tile.GetComponent<Tile>();
                tile_component.Instantiate(this, i * BoardSize + j, 0);
                tile_component.TileSelected += OnBuildingCreation;
                tile_component.BuildingSelected += OnBuildingSelected;
            }
        }
        navMesh.BuildNavMesh();
    }

    void GenerateRandomFeature()
    {
        if (random.Next(0, 2) == 0)
        {
            int i = 0;
            foreach (Transform tr in transform)
            {
                if (tr.gameObject.GetComponent<Tile>() is Tile tile)
                {
                    if (tile.IsEmpty()) { i++; }
                }
            }
            var selection = random.Next(0, i);
            foreach (Transform tr in transform)
            {
                if (tr.gameObject.GetComponent<Tile>() is Tile tile)
                {
                    if (tile.IsEmpty())
                    {
                        if (selection == 0)
                        {
                            if (random.Next(0, 4) == 0)
                            {
                                Instantiate(Fountain, tile.transform);
                            }
                            else
                            {
                                Instantiate(Trees[random.Next(0, Trees.Count())], tile.transform);
                            }
                        }
                        selection--;
                    }
                }
            }
        }
    }

    public void OnBuildingCreation(object sender, TileSelectedEvent e)
    {
        switch (CurrentPlayerAction)
        {
            case PlayerAction.Building:
                if (SelectedBuildingDefinition is BuildingDefinition definition)
                {
                    var building = CreateBuilding(e.Tile.transform);
                    building.GetComponent<Building>().Instantiate(definition);
                    building.AddComponent<MeshCollider>();
                    UpdateRequirements();
                    GenerateRandomFeature();
                    BuildingCreated?.Invoke(this, new());
                }
                break;
            default:
                break;
        }
    }

    public void OnBuildingSelected(object sender, BuildingSelectedEvent e)
    {
        switch (CurrentPlayerAction)
        {
            case PlayerAction.Building:
                break;
            case PlayerAction.Bombing:
                if (gameManager.TrySpendBomb())
                {
                    e.Tile.Demolish();
                    levelInfo.Refresh();
                    BombDetonated?.Invoke(this, new());
                }
                break;
            case PlayerAction.Info:
                BuildingHud?.Focus(e.Definition);
                break;

        }
    }

    (Tile, Tile) RandomBuildings()
    {
        List<Tile> tiles = new();
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Tile>() is Tile tile)
            {
                if (tile.GetBuildingDefinition() is not null)
                {
                    tiles.Add(tile);
                }
            }
        }
        var a = random.Next(0, tiles.Count());
        var b = random.Next(1, tiles.Count());
        return (tiles[a], tiles[(a + b) % tiles.Count()]);
    }

    bool ShouldGeneratePedestrians()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Tile>() is Tile tile)
            {
                if (tile.GetBuildingDefinition() is not null)
                {
                    i++;
                }
            }
        }
        return i > 1;
    }

    void Update()
    {
        if (ShouldGeneratePedestrians())
        {
            if (timeSinceLastPedestrian > 1)
            {
                var (source, destination) = RandomBuildings();
                var pedestrian = Instantiate(Pedestrian, source.transform.position + new Vector3(0, 0, 1), Quaternion.identity);
                pedestrian.GetComponent<NavMeshAgent>().SetDestination(destination.transform.position);
                timeSinceLastPedestrian -= 1;
            }
            timeSinceLastPedestrian += Time.deltaTime * PedestrianGeneration;
        }
    }

    public GameObject CreateBuilding(Transform transform)
    {
        return Instantiate(Building, transform);
    }
}
