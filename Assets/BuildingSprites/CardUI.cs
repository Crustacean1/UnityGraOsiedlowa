using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/*[Serializable]
public struct Card{
	public string Name;
	public float Height;
	public float Area;
	public int Floors;
	public int Residents;
};*/

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public TMP_Text Name;
	public TMP_Text Height;
	public TMP_Text Area;
	public TMP_Text Floors;
	public TMP_Text Residents;
	public Image CardImage;

	public void Instantiate(CardType cardInfo){
		Name.text = cardInfo.Name;
		Height.text = $"Height: {cardInfo.Height}";
		Area.text = $"Area: {cardInfo.Area}";
		Floors.text = $"Floors: {cardInfo.Floors}";
		Residents.text = $"People: {cardInfo.Residents}";
		CardImage.sprite = cardInfo.Image;
	}

	public void OnPointerEnter(PointerEventData eventData){
		gameObject.GetComponent<Image>().color = new Color32(50,50,50,255);
	}

	public void OnPointerExit(PointerEventData eventData){
		gameObject.GetComponent<Image>().color = new Color32(30,30,30,255);
	}

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
