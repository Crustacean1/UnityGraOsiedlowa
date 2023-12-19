using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

public class Board : MonoBehaviour
{
    public GameController gameManager;
    public int Width;
    public int Height;
    public GameObject Tile;
    public float Size;
    public NavMeshSurface navMesh;


    // Start is called before the first frame update
    void Start()
    {
        var center = new Vector3(Width - 1, 0, Height - 1) * Size * 0.5f;
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                var tile = Instantiate(Tile, new Vector3(i * Size, 0.9f, j * Size) - center, new Quaternion(Mathf.Sqrt(2) / 2, 0, 0, Mathf.Sqrt(2) / 2));
                tile.transform.parent = gameObject.transform;
                Tile tile_component = tile.GetComponent<Tile>();
                tile_component.gameManager = gameManager;
                tile_component.Instantiate(i * Height + j);
            }
        }
        navMesh.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
