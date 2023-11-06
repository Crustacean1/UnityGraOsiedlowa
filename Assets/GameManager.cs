using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BuildingCard
{
    public string Name;
    public int Floors;
    public int Residents;
    public float Height;
    public float Area;

    public Sprite Image;
    public Mesh BuildingMesh;
}

public class GameManager : MonoBehaviour
{
    public List<BuildingCard> CardPrefabs = new();
    public List<int> CurrentDeck = new();
    public BuildingCard ? CurrentCard = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
