using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Specialized;
using System.Security.Permissions;
using TMPro;

public class Deck : MonoBehaviour
{
    private int selectedCard = -1;

    public GameController gameManager;
    public Board GameBoard;

    public GameObject DeckUi;
    public GameObject UICard;
    public GameObject ActionUi;
    public GameObject Player;
    public GameObject EscapeInfo;
    public GameObject BombButton;

    public int CardsInHand;

    public float BottomMargin;
    public float CardGap;

    public List<BuildingDefinition> BuildingDefinitions;
    public BuildingDefinition SelectedBuildingDefinition => selectedCard > 0 ? BuildingDefinitions[selectedCard] : null;

    public bool IsUiSelected
    {
        get
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadBuildingDefinitions();
        var hand = DrawRandomHand();
        RecreateDeck(hand);
        RefreshBombButton();

        GameBoard.BombDetonated += OnBombDetonated;
        GameBoard.BuildingCreated += OnBuildingCreated;
    }

    public void ShowEndTurnActions()
    {
        GameBoard.CurrentPlayerAction = PlayerAction.Info;
        GameBoard.SelectedBuildingDefinition = null;
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
        GameBoard.CurrentPlayerAction = PlayerAction.Info;

        ActionUi.SetActive(false);
        GameBoard.SelectedBuildingDefinition = null;
        RecreateDeck(DrawRandomHand());
        DeckUi.SetActive(true);
    }

    public void BombBuilding()
    {
        GameBoard.CurrentPlayerAction = PlayerAction.Bombing;
    }

    public void RefreshBombButton()
    {
        var bombsLeft = gameManager.LevelInfo.Bombs;
        BombButton.transform.GetChild(0).GetComponent<TMP_Text>().text = $"Bomb Building\n{bombsLeft} Left";
        BombButton.GetComponent<Button>().interactable = bombsLeft > 0;
    }

    public void WalkAround()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Player.SetActive(true);
        Player.transform.localPosition = new Vector3(-8, 0.25f, -8);

        foreach (Transform child in ActionUi.transform.parent.parent)
        {
            child.gameObject.SetActive(false);
        }
        EscapeInfo.SetActive(true);

    }

    public void Exit()
    {
        UnityEngine.Debug.Log("Ending play");
        if (FindObjectsOfType<GodScript>().SingleOrDefault() is GodScript godScript)
        {
            UnityEngine.Debug.Log("Done");
            godScript.FinishGame();
        }
    }

    GameObject createCardUi(BuildingDefinition building, Vector3 position, int instance)
    {
        var card = Instantiate(UICard, DeckUi.transform);
        card.transform.localPosition = new Vector3(0, 0, 0);
        for (int i = 0; i < CardsInHand; i++)
        {
            if (card.GetComponent<CardUI>() is CardUI cardUI)
            {
                cardUI.Instantiate(building);
                cardUI.CardSelected += OnCardSelected;
            }
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
        //BuildingDefinitions = JsonUtility.FromJson<BuildingContainer>(buildings?.text).Buildings;
        BuildingDefinitions = JsonConvert.DeserializeObject<BuildingContainer>(buildings.text).Buildings;
        //UnityEngine.Debug.Log($"Found source: {buildings.text}");

        //UnityEngine.Debug.Log($"Found source: {BuildingDefinitions.Count()}");
    }

    void Update()
    {
        GameBoard.RaycastEnabled = !IsUiSelected;
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

    public void OnBombDetonated(object sender, BombDetonated e)
    {
        RefreshBombButton();
    }

    public void OnBuildingCreated(object sender, BuildingCreatedEvent e)
    {
        ShowEndTurnActions();
    }

    public void OnCardSelected(object sender, BuildingCardSelectedEvent e)
    {
        GameBoard.SelectedBuildingDefinition = e.definition;
        GameBoard.CurrentPlayerAction = PlayerAction.Building;
    }
}
