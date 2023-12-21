using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Specialized;
using System.Security.Permissions;

public class Deck : MonoBehaviour
{
    private int selectedCard = -1;

    public GameController gameManager;
    public GameObject DeckUi;
    public GameObject UICard;
    public GameObject ActionUi;
    public GameObject Player;
    public GameObject EscapeInfo;

    public int CardsInHand;

    public float BottomMargin;
    public float CardGap;

    public List<BuildingDefinition> BuildingDefinitions;
    public BuildingDefinition SelectedBuildingDefinition => selectedCard > 0 ? BuildingDefinitions[selectedCard] : null;
    public bool BombSelected;

    // Start is called before the first frame update
    void Start()
    {
        LoadBuildingDefinitions();
        var hand = DrawRandomHand();
        RecreateDeck(hand);
    }

    public void ShowEndTurnActions()
    {
        selectedCard = -1;
        DeckUi.SetActive(false);
        ActionUi.SetActive(true);
    }

    public void RecreateDeck(IEnumerable<BuildingDefinition> definitions)
    {
        foreach (Transform card in DeckUi.transform)
        {
            Destroy(card.gameObject);
        }

        if (DeckUi.GetComponent<RectTransform>() is RectTransform rectTr)
        {
            float deckWidth = (definitions.Count() - 1) * CardGap;
            Vector3 left = new Vector3((-deckWidth) * 0.5f, 0, 0);
            Vector3 position = left;
            int i = 0;
            foreach (var card in definitions)
            {
                var uiCard = createCardUi(card, new Vector3(0, 0, 0), i++);
                uiCard.transform.SetParent(DeckUi.transform);
                uiCard.GetComponent<RectTransform>().anchoredPosition3D = position;
                position += new Vector3(CardGap, 0, 0);
            }
        }
    }

    public void DrawCards()
    {
        BombSelected = false;
        ActionUi.SetActive(false);
        RecreateDeck(DrawRandomHand());
        DeckUi.SetActive(true);
    }

    public void BombBuilding()
    {
        BombSelected = true;
    }

    public void WalkAround()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Player.SetActive(true);

        foreach (Transform child in ActionUi.transform.parent.parent)
        {
            child.gameObject.SetActive(false);
        }
        EscapeInfo.SetActive(true);

    }

    public void Exit()
    {

    }

    GameObject createCardUi(BuildingDefinition building, Vector3 position, int instance)
    {
        var card = Instantiate(UICard, DeckUi.transform);
        card.transform.localPosition = new Vector3(0, 0, 0);
        for (int i = 0; i < 4; i++)
        {
            card.GetComponent<CardUI>()?.Instantiate(this, building);
        }

        return card;
    }

    public void SelectCard(string name)
    {
        selectedCard = BuildingDefinitions.FindIndex(b => b.Name == name);
    }

    public IEnumerable<BuildingDefinition> DrawRandomHand()
    {
        List<int> newCards = new();
        System.Random random = new();

        while (newCards.Count() < CardsInHand)
        {
            int pretender = random.Next(0, BuildingDefinitions.Count());
            if (!newCards.Contains(pretender)) { newCards.Add(pretender); }
        }

        return newCards.Select(i => BuildingDefinitions[i]).ToList();
    }


    private void LoadBuildingDefinitions()
    {
        var buildings = Resources.Load<TextAsset>("BuildingDefinitions/buildings");
        BuildingDefinitions = JsonUtility.FromJson<BuildingContainer>(buildings?.text).Buildings;
        UnityEngine.Debug.Log($"Found source: {buildings.text}");
        UnityEngine.Debug.Log($"Found source: {BuildingDefinitions.Count()}");
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Player.SetActive(false);
            foreach (Transform child in ActionUi.transform.parent.parent)
            {
                child.gameObject.SetActive(true);
            }
            EscapeInfo.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
        }
    }
}
