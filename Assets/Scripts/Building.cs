using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingDefinition definition;

    public MeshFilter Mesh;
    public BuildingDefinition Definition => definition;
    
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
