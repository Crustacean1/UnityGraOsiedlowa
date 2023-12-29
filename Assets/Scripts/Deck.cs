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

/// <summary>
/// Controls the deck of cards, card interactions, and player actions.
/// </summary>
public class Deck : MonoBehaviour
{
    /// <summary>
    /// The index of the selected card.
    /// </summary>
    private int selectedCard = -1;

    /// <summary>
    /// Reference to the game's controller.
    /// </summary>
    public GameController gameManager;

    /// <summary>
    /// Reference to the game board.
    /// </summary>
    public Board GameBoard;

    /// <summary>
    /// The UI element for the deck.
    /// </summary>
    public GameObject DeckUi;

    /// <summary>
    /// The UI element for a card.
    /// </summary>
    public GameObject UICard;

    /// <summary>
    /// The UI element for in-game actions.
    /// </summary>
    public GameObject ActionUi;

    /// <summary>
    /// The player object in the game.
    /// </summary>
    public GameObject Player;

    /// <summary>
    /// Information about escape options.
    /// </summary>
    public GameObject EscapeInfo;

    /// <summary>
    /// The button to trigger bombing action.
    /// </summary>
    public GameObject BombButton;

    /// <summary>
    /// Number of cards in the player's hand.
    /// </summary>
    public int CardsInHand;

    /// <summary>
    /// Margin from the bottom for the cards.
    /// </summary>
    public float BottomMargin;

    /// <summary>
    /// Gap between cards.
    /// </summary>
    public float CardGap;

    /// <summary>
    /// The list of building definitions.
    /// </summary>
    public List<BuildingDefinition> BuildingDefinitions;

    /// <summary>
    /// The currently selected building definition.
    /// </summary>
    public BuildingDefinition SelectedBuildingDefinition => selectedCard > 0 ? BuildingDefinitions[selectedCard] : null;

    /// <summary>
    /// Checks if the UI is selected.
    /// </summary>
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

    /// <summary>
    /// Shows the end turn actions.
    /// </summary>
    public void ShowEndTurnActions()
    {
        GameBoard.CurrentPlayerAction = PlayerAction.Info;
        GameBoard.SelectedBuildingDefinition = null;
        DeckUi.SetActive(false);
        ActionUi.SetActive(true);
    }

    /// <summary>
    /// Event handler for deck recreation.
    /// </summary>
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

    /// <summary>
    /// Event handler for drawing cards.
    /// </summary>
    public void DrawCards()
    {
        GameBoard.CurrentPlayerAction = PlayerAction.Info;

        ActionUi.SetActive(false);
        GameBoard.SelectedBuildingDefinition = null;
        RecreateDeck(DrawRandomHand());
        DeckUi.SetActive(true);
    }

    /// <summary>
    /// Event handler for bomb placement.
    /// </summary>
    public void BombBuilding()
    {
        GameBoard.CurrentPlayerAction = PlayerAction.Bombing;
    }

    /// <summary>
    /// Event handler for refreshing bomb button.
    /// </summary>
    public void RefreshBombButton()
    {
        var bombsLeft = gameManager.LevelInfo.Bombs;
        BombButton.transform.GetChild(0).GetComponent<TMP_Text>().text = $"Bomb Building\n{bombsLeft} Left";
        BombButton.GetComponent<Button>().interactable = bombsLeft > 0;
    }

    /// <summary>
    /// Event handler for walking.
    /// </summary>
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

    /// <summary>
    /// Event handler for exit.
    /// </summary>
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

    /// <summary>
    /// Script for selecting a card.
    /// </summary>
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

    /// <summary>
    /// Definitions for building loading.
    /// </summary>
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

    /// <summary>
    /// Event handler for bomb detonation.
    /// </summary>
    public void OnBombDetonated(object sender, BombDetonated e)
    {
        RefreshBombButton();
    }

    /// <summary>
    /// Event handler for building creation.
    /// </summary>
    public void OnBuildingCreated(object sender, BuildingCreatedEvent e)
    {
        ShowEndTurnActions();
    }

    /// <summary>
    /// Event handler for card selection.
    /// </summary>
    public void OnCardSelected(object sender, BuildingCardSelectedEvent e)
    {
        GameBoard.SelectedBuildingDefinition = e.definition;
        GameBoard.CurrentPlayerAction = PlayerAction.Building;
    }
}
