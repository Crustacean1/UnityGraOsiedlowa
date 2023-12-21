using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text Name;
    public TMP_Text Height;
    public TMP_Text Area;
    public TMP_Text Floors;
    public TMP_Text Residents;
    public Image CardImage;

    private Deck CardDeck;
    private BuildingDefinition buildingDefinition;
    private bool hover;


    public void Instantiate(Deck cardDeck, BuildingDefinition cardInfo)
    {
        this.CardDeck = cardDeck;
        buildingDefinition = cardInfo;

        Name.text = cardInfo.Name;
        Height.text = $"Height: ";
        Area.text = $"Area: ";
        Floors.text = $"Floors: ";
        Residents.text = $"People: ";

        UnityEngine.Debug.Log($"BuildingImages/{cardInfo.Sprite}");
        var sprite = Resources.Load<Sprite>($"BuildingImages/{cardInfo.Sprite}");
        if(sprite is not null)
        {
            CardImage.sprite = sprite;
        }
        else
        {
            UnityEngine.Debug.Log("Failed to load sprite");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().color = new Color32(50, 50, 50, 255);
        hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().color = new Color32(30, 30, 30, 255);
        hover = false;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && hover)
        {
            CardDeck.SelectCard(buildingDefinition.Name);
        }
    }
}
