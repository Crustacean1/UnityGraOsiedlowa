using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int id;
    public MeshRenderer Border;
    public MeshRenderer Center;
    public bool IsActive;
    public Collider collider;
    public GameManager gameManager;
    private bool hover;
    public MeshFilter building;
    public MeshRenderer renderer;

    enum TileState
    {
        Empty,
        Occupied,
    }
    TileState State = TileState.Empty;

    public void Instantiate(int id)
    {
        this.id = id;
    }

    // Start is called before the first frame update
    void Start()
    {
        Border.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (collider.Raycast(ray, out var hit, Mathf.Infinity))
        {
            if(!hover)
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

        if (hover && Input.GetMouseButtonDown(0)){
            OnClick();
        }
    }
    void OnHoverEntry()
    {
        Border.enabled = true;
        switch (State)
        {
            case TileState.Empty:
                if (gameManager.CurrentCard is BuildingCard CurrentCard)
                {
                    building.mesh = CurrentCard.BuildingMesh;
                    renderer.enabled = true;
                }
                break;
            case TileState.Occupied:
                Border.enabled = true;
                Border.material.color = Color.red;
                break;
        }

    }
    void OnHoverExit()
    {
        Border.enabled = false;
        switch (State)
        {
            case TileState.Empty:
                renderer.enabled = false;
                break;
            case TileState.Occupied:
                break;
        }

    }
    void OnClick()
    {
        UnityEngine.Debug.Log($"Pressed on hover: {id}");
        switch (State)
        {
            case TileState.Empty:
                State = TileState.Occupied;
                break;
            case TileState.Occupied:
                break;
        }
    }
}
