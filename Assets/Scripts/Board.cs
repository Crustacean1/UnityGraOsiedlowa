using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;


public class Board : MonoBehaviour
{
    public GameController gameManager;
    public LevelInfoHud levelInfo;
    public GameObject Building;
    public BuildingHud BuildingHud;
    public int BoardSize;
    public GameObject Tile;
    public float Size;
    public NavMeshSurface navMesh;

    public Deck cardDeck;

    // Start is called before the first frame update
    void Start()
    {
        if (cardDeck is not null)
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
                    tile_component.Instantiate(cardDeck, this, i * BoardSize + j, 0);
                    tile_component.TileSelected += OnBuildingCreation;
                    tile_component.BuildingSelected += OnBuildingSelected;
                }
            }
        }
        navMesh.BuildNavMesh();
    }

    public void OnBuildingCreation(object sender, TileSelectedEvent e)
    {
        if (cardDeck.SelectedBuildingDefinition is BuildingDefinition definition)
        {
            CreateBuilding(e.Tile.transform).GetComponent<Building>().Instantiate(definition);
        }
        cardDeck.ShowEndTurnActions();
    }

    public void OnBuildingSelected(object sender, BuildingSelectedEvent e)
    {
        if (cardDeck.BombSelected)
        {
            if (gameManager.TrySpendBomb())
            {
                e.Tile.Demolish();
                levelInfo.Refresh();
            }
        }
        else
        {
            BuildingHud?.Focus(e.Definition);
        }
    }

    public GameObject CreateBuilding(Transform transform)
    {
        return Instantiate(Building, transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
