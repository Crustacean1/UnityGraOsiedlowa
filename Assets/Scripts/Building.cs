using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class representing a building in the game.
/// </summary>
public class Building : MonoBehaviour
{
    /// <summary>
    /// Private field storing the building definition.
    /// </summary>
    private BuildingDefinition definition;

    /// <summary>
    /// Public property representing the mesh filter of the building.
    /// </summary>
    public MeshFilter Mesh;

    /// <summary>
    /// Public property providing access to the building's definition.
    /// </summary>
    public BuildingDefinition Definition => definition;

    /// <summary>
    /// Instantiates the building with the specified building definition.
    /// </summary>
    /// <param name="buildingDefinition">The definition of the building to instantiate.</param>
    public void Instantiate(BuildingDefinition buildingDefinition)
    {
        definition = buildingDefinition;

        transform.localScale = new Vector3(0.05f, 0.05f,0.05f);
        transform.localPosition = new Vector3(0, 0.1f, 0);
        var meshResource = Resources.Load<Mesh>($"BuildingMeshes/{buildingDefinition.Mesh}");
        if(meshResource is Mesh mesh)
        {
            UnityEngine.Debug.Log($"Loading mesh");
            Mesh.mesh = mesh;
        }
        else
        {
            UnityEngine.Debug.Log($"Failed to load mesh BuildingMeshes/{buildingDefinition.Mesh}");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
