using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;

public class TileSelectedEvent : EventArgs
{
    public Tile Tile;
}

public class BuildingSelectedEvent : EventArgs
{
    public Tile Tile;
    public BuildingDefinition Definition;
}

public class Tile : MonoBehaviour
{
    private bool hover;
    private MeshRenderer tileMesh;
    private Deck cardDeck;
    private Board board;

    public int id;
    public bool IsActive;
    public MeshCollider collider;

    public Material SelectionMaterial;
    public MeshFilter building;
    public MeshRenderer mesh_renderer;
    public List<GameObject> Landscapes;

    public event EventHandler<BuildingSelectedEvent> BuildingSelected;
    public event EventHandler<TileSelectedEvent> TileSelected;

    public void Instantiate(Deck cardDeck, Board board, int id, int landscape)
    {
        this.id = id;
        this.cardDeck = cardDeck;
        this.board = board;

        var tile = Instantiate(Landscapes[landscape], gameObject.transform);
        tile.transform.localPosition = new Vector3(0, 0, 0);
        tile.transform.localRotation = Quaternion.identity;
        tileMesh = tile.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        collider = tile.GetComponent<MeshCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (hover)
            {
                OnClick();
            }
        }
    }

    public void Demolish()
    {
        foreach (Transform building in transform)
        {
            if (building.gameObject.GetComponent<Building>() is not null)
            {
                Destroy(building.gameObject);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (collider.Raycast(ray, out var hit, Mathf.Infinity))
        {
            if (!hover)
            {
                hover = true;
                OnHoverEntry();
            }
        }
        else
        {
            if (hover)
            {
                OnHoverExit();
                hover = false;
            }
        }
    }
    void OnHoverEntry()
    {
        var color = Color.white;
        if (cardDeck.SelectedBuildingDefinition is not null)
        {
            color = transform.childCount > 3 ? Color.red : Color.yellow;
        }
        var materials = new Material[] { tileMesh.material, SelectionMaterial };
        materials[1].color = color;
        tileMesh.materials = materials;
    }

    void OnHoverExit()
    {
        var materials = new Material[] { tileMesh.material };
        tileMesh.materials = materials;
    }

    void OnClick()
    {
        if (cardDeck.SelectedBuildingDefinition is BuildingDefinition definition)
        {
            TileSelected?.Invoke(this, new TileSelectedEvent { Tile = this });
        }

        else
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Building>() is Building building)
                {
                    BuildingSelected?.Invoke(this, new BuildingSelectedEvent
                    {
                        Tile = this,
                        Definition = building.Definition
                    });
                }
            }
        }
    }
}
