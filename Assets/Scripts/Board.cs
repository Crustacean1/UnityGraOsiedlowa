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

public class BuildingUpdatedEvent
{

}

public enum PlayerAction
{
    Building,
    RoadBuilding,
    Updating,
    Bombing,
    Info
}

public class Board : MonoBehaviour
{
    private System.Random random = new System.Random();

    private IList<Road> Roads = new List<Road>();
    private Road? SelectedRoad = null;
    private IList<Tile> Tiles = new List<Tile>();
    private Tile? SelectedTile = null;

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
    public GameObject Road;

    public float Size;
    public NavMeshSurface navMesh;

    public BuildingDefinition SelectedBuildingDefinition { get; set; }
    public PlayerAction CurrentPlayerAction { get; set; }

    public event System.EventHandler<BombDetonated> BombDetonated;
    public event System.EventHandler<BuildingCreatedEvent> BuildingCreated;
    public event System.EventHandler<BuildingUpdatedEvent> BuildingUpdated;

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
                            value.Current += property.Value[definition.Level];
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
                var tilePosition = rowStart + j * new Vector3(Size, 0, 0);
                var tile = Instantiate(Tile, tilePosition, Quaternion.identity);
                tile.transform.parent = gameObject.transform;
                tile.transform.localRotation = Quaternion.Euler(Vector3.up * 30);

                Tile tile_component = tile.GetComponent<Tile>();
                tile_component.Instantiate(i * BoardSize + j, 0);
                tile_component.TileSelected += OnBuildingCreation;
                tile_component.BuildingSelected += OnBuildingSelected;

                Tiles.Add(tile_component);

                if (i != 0 && (j + 1 != BoardSize + BoardSize - 1 - Mathf.Abs(i - BoardSize + 1) || i > BoardSize - 1))
                {
                    var orientation = Quaternion.AngleAxis(60, Vector3.up);
                    var road = Instantiate(Road, tilePosition + orientation * new Vector3(Size * 0.5f, 0.05f, 0), orientation, transform);
                    Roads.Add(road.GetComponent<Road>());
                }

                if (j + 1 != BoardSize + BoardSize - 1 - Mathf.Abs(i - BoardSize + 1))
                {
                    var orientation = Quaternion.AngleAxis(0, Vector3.up);
                    var road = Instantiate(Road, tilePosition + orientation * new Vector3(Size * 0.5f, 0.05f, 0), orientation, transform);
                    Roads.Add(road.GetComponent<Road>());
                }
                if (i + 1 != BoardSize * 2 - 1 && (j + 1 != BoardSize + BoardSize - 1 - Mathf.Abs(i - BoardSize + 1) || i < BoardSize - 1))
                {
                    var orientation = Quaternion.AngleAxis(-60, Vector3.up);
                    var road = Instantiate(Road, tilePosition + orientation * new Vector3(Size * 0.5f, 0.05f, 0), orientation, transform);
                    Roads.Add(road.GetComponent<Road>());
                }
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
        UnityEngine.Debug.Log($"Current action: {CurrentPlayerAction}");
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
        if (Input.GetMouseButtonDown(0))
        {
            OnBuildingSelected(this, new());
        }
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
    public void OnBuildingSelected(object sender, BuildingSelectedEvent e)
    {
        switch (CurrentPlayerAction)
        {
            case PlayerAction.Building:
                if (SelectedTile is not null)
                {
                    SelectedTile?.OnClick();
                }
                break;
            case PlayerAction.Bombing:
                if (gameManager.TrySpendBomb())
                {
                    SelectedTile.Demolish();
                    levelInfo.Refresh();
                    BombDetonated?.Invoke(this, new());
                }
                break;
            case PlayerAction.RoadBuilding:
                if (SelectedRoad is not null)
                {
                    SelectedRoad?.OnClick();
                    BuildingCreated?.Invoke(this, null);
                }
                break;
            case PlayerAction.Updating:
                if (SelectedTile is not null)
                {
                    var obj = SelectedTile.transform.Cast<Transform>().FirstOrDefault((Transform child) => child.GetComponent<Building>() != null);
                    if (obj?.GetComponent<Building>() is Building building)
                    {
                        building.AnimateUpdate();
                    }
                    BuildingCreated?.Invoke(this, null);
                }
                break;
            case PlayerAction.Info:
                var obj2 = SelectedTile.transform.Cast<Transform>().FirstOrDefault((Transform child) => child.GetComponent<Building>() != null);
                if (obj2?.GetComponent<Building>() is Building building2)
                {
                    BuildingHud?.Focus(building2.definition);
                }
                break;

        }
    }


    void FixedUpdate()
    {
        if (CurrentPlayerAction == PlayerAction.RoadBuilding)
        {
            var newRoadSelection = Roads.Where(r => r.CheckForIntersection()).FirstOrDefault();
            if (newRoadSelection != SelectedRoad)
            {
                SelectedRoad?.OnHoverExit();
                newRoadSelection?.OnHoverEntry();
                SelectedRoad = newRoadSelection;
            }
        }

        if (CurrentPlayerAction != PlayerAction.RoadBuilding)
        {
            var newTileSelection = Tiles.Where(t => t.CheckForIntersection()).FirstOrDefault();
            if (newTileSelection != SelectedTile)
            {
                SelectedTile?.OnHoverExit();
                newTileSelection?.OnHoverEntry(this);
                SelectedTile = newTileSelection;
            }
        }
    }

    public GameObject CreateBuilding(Transform transform)
    {
        return Instantiate(Building, transform);
    }
}
