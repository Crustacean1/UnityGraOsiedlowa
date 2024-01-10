using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    private bool Created;

    public MeshCollider RayCollider;
    public MeshRenderer RoadMesh;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public bool CheckForIntersection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return RayCollider.Raycast(ray, out var hit, Mathf.Infinity);
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

    public void OnHoverEntry()
    {
        if (!Created)
        {
            RoadMesh.enabled = true;
        }
    }

    public void OnHoverExit()
    {
        if (!Created)
        {
            RoadMesh.enabled = false;
        }
    }

    public void OnClick()
    {
        Created = true;
        RoadMesh.enabled = true;
    }
}
