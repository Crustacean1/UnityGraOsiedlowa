using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Represents a parameter with a name, minimum, and maximum values.
/// </summary>
[Serializable]
public class Parameter
{
    public string Name;
    public float Min;
    public float Max;
}

/// <summary>
/// Controls the behavior of a level selection button.
/// </summary>
public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool hover;
    private Action onSelect;
    private int id;

    /// <summary>
    /// Accessor for the button's ID.
    /// </summary>
    public int Id => id;

    /// <summary>
    /// Gets or sets the selection status of the button.
    /// </summary>
    public bool IsSelected { get; set; }

    /// <summary>
    /// List of parameters associated with the button.
    /// </summary>
    public List<Parameter> Parameters;

    /// <summary>
    /// Text displaying the button's name.
    /// </summary>
    public TMP_Text Text;

    /// <summary>
    /// Icon representing the button.
    /// </summary>
    public Image Icon;

    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// Initializes the level button with specified ID, name, and action on selection.
    /// </summary>
    /// <param name="id">ID of the button.</param>
    /// <param name="name">Name of the button.</param>
    /// <param name="onSelect">Action to perform on selection.</param>
    public void Instantiate(int id, string name, Action onSelect)
    {
        this.id = id;
        this.onSelect = onSelect;
        this.Text.text = name;
        if (gameObject.GetComponent<Button>() is Button button)
        {
            button.onClick.AddListener(() => onSelect());
        }
        var sprite = Resources.Load<Sprite>($"DifficultyIcons/{name}") ?? Resources.Load<Sprite>("DifficultyIcons/default");
        Icon.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        var color = IsSelected ? Color.red : Color.white;
        color = hover ? Color.green : color;
        Text.color = color;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        hover = true;
    }

    public void OnPointerExit(PointerEventData data)
    {
        hover = false;
    }
}
