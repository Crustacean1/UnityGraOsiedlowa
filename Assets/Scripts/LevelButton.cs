using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[Serializable]
public class Parameter
{
    public string Name;
    public float Min;
    public float Max;
}

public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool hover;
    private Action onSelect;
    private int id;

    public int Id => id;
    public bool IsSelected { get; set; }
    public List<Parameter> Parameters;

    public TMP_Text Text;
    public Image Icon;

    // Start is called before the first frame update
    void Start()
    {

    }

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
