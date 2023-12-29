using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages the main menu interface and interactions.
/// </summary>
public class MainMenu : MonoBehaviour
{
    private int selectedLevel = -1;
    private LevelButton[] levels;

    /// <summary>
    /// Text displaying the username in the main menu.
    /// </summary>
    public TMP_Text Username;

    /// <summary>
    /// Panel containing level buttons in the main menu.
    /// </summary>
    public Transform LevelPanel;

    /// <summary>
    /// Prefab for the level label button.
    /// </summary>
    public GameObject LevelLabelPrefab;

    /// <summary>
    /// Reference to the main GodScript managing game states.
    /// </summary>
    public GodScript godScript;

    // Start is called before the first frame update
    void Start()
    {
        CreateLevelButtons();
    }

    void CreateLevelButtons()
    {
        float gap = 40;
        var start = new Vector3(0, (godScript.LevelDefinitions.Count() - 1) * gap * 0.5f, 0);

        Func<LevelDefinition, int, LevelButton> createLevelButton = (LevelDefinition definition, int id) => InstantiateLevelButton(definition, id, start, gap);
        levels = godScript.LevelDefinitions.Select(createLevelButton).ToArray();
    }

    LevelButton InstantiateLevelButton(LevelDefinition definition, int id, Vector3 position, float gap)
    {
        var level = Instantiate(LevelLabelPrefab);
        level.transform.SetParent(LevelPanel);

        if (level.GetComponent<RectTransform>() is RectTransform rectTransform)
        {
            rectTransform.anchoredPosition = (position - new Vector3(0, id * gap, 0));
        }
        if (level.GetComponent<LevelButton>() is LevelButton levelButton)
        {
            levelButton.Instantiate(id, definition.Name, () => OnLevelChange(id));
            return levelButton;
        }
        return null;
    }

    void OnLevelChange(int id)
    {
        selectedLevel = id;
        foreach (var level in levels)
        {
            level.IsSelected = (level.Id == id);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Initiates the game with selected username and difficulty level.
    /// </summary>
    public void StartGame()
    {
        if (Username.text.Length < 2)
        {
            UnityEngine.Debug.Log("No username selected");
            return;
        }
        if (selectedLevel == -1)
        {
            UnityEngine.Debug.Log("No difficulty selected");
            return;
        }

        godScript.StartGame(new Player { Name = Username.text }, selectedLevel);
    }
}
