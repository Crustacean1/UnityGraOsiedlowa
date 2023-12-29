using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Event triggered when a building card is selected.
/// </summary>
public class BuildingCardSelectedEvent
{
    public BuildingDefinition definition;
}

/// <summary>
/// UI script for displaying information on a building card.
/// </summary>
public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text Name;
    public TMP_Text Height;
    public TMP_Text Area;
    public TMP_Text Floors;
    public TMP_Text Residents;
    public Image CardImage;

    /// <summary>
    /// Event triggered when the card is selected.
    /// </summary>
    public event EventHandler<BuildingCardSelectedEvent> CardSelected;

    private BuildingDefinition buildingDefinition;
    private bool hover;

    /// <summary>
    /// Sets up the card UI with building information.
    /// </summary>
    /// <param name="cardInfo">The building information to display on the card.</param>
    public void Instantiate(BuildingDefinition cardInfo)
    {
        buildingDefinition = cardInfo;

        Name.text = cardInfo.Name;
        Height.text = $"Height: ";
        Area.text = $"Area: ";
        Floors.text = $"Floors: ";
        Residents.text = $"People: ";

        UnityEngine.Debug.Log($"BuildingImages/{cardInfo.Sprite}");
        var sprite = Resources.Load<Sprite>($"BuildingImages/{cardInfo.Sprite}");
        if (sprite is not null)
        {
            CardImage.sprite = sprite;
        }
        else
        {
            UnityEngine.Debug.Log("Failed to load sprite");
        }
    }

    /// <summary>
    /// Called when the pointer enters the card's area.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().color = new Color32(50, 50, 50, 255);
        hover = true;
    }

    /// <summary>
    /// Called when the pointer exits the card's area.
    /// </summary>
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
            CardSelected?.Invoke(this, new BuildingCardSelectedEvent { definition = buildingDefinition });
        }
    }
}
