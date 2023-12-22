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
    private Board board;

    public int id;
    public bool IsActive;
    public MeshCollider tileCollider;

    public Material SelectionMaterial;
    public MeshFilter building;
    public MeshRenderer mesh_renderer;
    public List<GameObject> Landscapes;

    public event EventHandler<BuildingSelectedEvent> BuildingSelected;
    public event EventHandler<TileSelectedEvent> TileSelected;

    public void Instantiate(Board board, int id, int landscape)
    {
        this.id = id;
        this.board = board;

        var tile = Instantiate(Landscapes[landscape], gameObject.transform);
        tile.transform.localPosition = new Vector3(0, 0, 0);
        tile.transform.localRotation = Quaternion.identity;
        tileMesh = tile.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        tileCollider = tile.GetComponent<MeshCollider>();
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
        if (board.RaycastEnabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (tileCollider.Raycast(ray, out var hit, Mathf.Infinity))
            {
                if (!hover)
                {
                    hover = true;
                    OnHoverEntry();
                }
                return;
            }
        }
        if (hover)
        {
            OnHoverExit();
            hover = false;
        }

    }

    public bool IsEmpty()
    {
        return transform.childCount < 4;
    }

    public BuildingDefinition GetBuildingDefinition()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Building>() is Building building)
            {
                return building.Definition;
            }
        }
        return null;
    }

    void OnHoverEntry()
    {
        var color = Color.white;

        if (board.SelectedBuildingDefinition is not null && board.CurrentPlayerAction == PlayerAction.Building)
        {
            color = GetBuildingDefinition() != null ? Color.yellow : Color.green;
        }
        else if (board.CurrentPlayerAction == PlayerAction.Bombing)
        {
            color = GetBuildingDefinition() != null ? Color.red : Color.grey;
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
        if (GetBuildingDefinition() is BuildingDefinition definition)
        {
            BuildingSelected?.Invoke(this, new BuildingSelectedEvent
            {
                Tile = this,
                Definition = definition
            });
        }
        else
        {
            TileSelected?.Invoke(this, new TileSelectedEvent { Tile = this });
        }
    }
}
