using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum CardType
{
    Road,
    Upgrade,
    Building
};

public class BuildingCardSelectedEvent
{
    public CardType Type;
    public BuildingDefinition definition;
}

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text Name;
    public TMP_Text Height;
    public TMP_Text Area;
    public TMP_Text Floors;
    public TMP_Text Residents;
    public Image CardImage;

    public event EventHandler<BuildingCardSelectedEvent> CardSelected;

    private BuildingDefinition buildingDefinition;
    private CardType Type;
    private bool hover;

    public void Instantiate(CardDefinition cardInfo)
    {
        Type = cardInfo.Type;
        if (cardInfo.Type == CardType.Building)
        {
            buildingDefinition = cardInfo.Definition;

            Name.text = buildingDefinition.Name;
            Height.text = $"Height: ";
            Area.text = $"Area: ";
            Floors.text = $"Floors: ";
            Residents.text = $"People: ";

            UnityEngine.Debug.Log($"BuildingImages/{buildingDefinition.Sprite}");
            var sprite = Resources.Load<Sprite>($"BuildingImages/{buildingDefinition.Sprite}");
            if (sprite is not null)
            {
                CardImage.sprite = sprite;
            }
            else
            {
                UnityEngine.Debug.Log("Failed to load sprite");
            }
        }
        if (cardInfo.Type == CardType.Road)
        {
            buildingDefinition = null;

            Name.text = "Road";

            var sprite = Resources.Load<Sprite>($"BuildingImages/road");
            if (sprite is not null)
            {
                CardImage.sprite = sprite;
            }
            else
            {
                UnityEngine.Debug.Log("Failed to load sprite");
            }
        }
        if (cardInfo.Type == CardType.Upgrade)
        {
            buildingDefinition = null;

            Name.text = "Upgrade";

            var sprite = Resources.Load<Sprite>($"BuildingImages/upgrade");
            if (sprite is not null)
            {
                CardImage.sprite = sprite;
            }
            else
            {
                UnityEngine.Debug.Log("Failed to load sprite");
            }
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
            CardSelected?.Invoke(this, new BuildingCardSelectedEvent
            {
                definition = buildingDefinition,
                Type = Type
            });
        }
    }
}
