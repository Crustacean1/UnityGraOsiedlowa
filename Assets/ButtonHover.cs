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

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool hover;
    MainMenu menu;

    public int Id;
    public TMP_Text Text;

    public List<Parameter> Parameters;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Instantiate(int id, string name, MainMenu menu)
    {
        this.Id = id;
        this.Text.text = name;
        this.menu = menu;
        if(gameObject.GetComponent<Button>() is Button button)
        {
            button.onClick.AddListener(() => menu.SetDifficulty(id));
        }
    }

    // Update is called once per frame
    void Update()
    {
        var color = Selected() ? Color.red : Color.white;
        color = hover ? Color.green : color;
        Text.color = color;
    }

    bool Selected() { return menu.Difficulty == Id; }

    public void OnPointerEnter(PointerEventData data)
    {
        hover = true;
    }

    public void OnPointerExit(PointerEventData data)
    {
        hover = false;
    }
}
