using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Tile : MonoBehaviour
{
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
        Selected
    }
    TileState State = TileState.Empty;

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
                Border.enabled = true;
                if (gameManager.CurrentCard is BuildingCard CurrentCard)
                { 
                    building.mesh = CurrentCard.BuildingMesh;
                    renderer.enabled = true;
                }
            }
        }
        else
        {
            if(hover)
            {
                hover = false;
                Border.enabled = false;
                renderer.enabled = false;
            }
        }
        if(Input.GetMouseButtonDown(0))
            {
            State = TileState.Selected;
            }
    }
}
