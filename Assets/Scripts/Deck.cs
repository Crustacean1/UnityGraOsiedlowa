using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Specialized;
using System.Security.Permissions;

public class Deck : MonoBehaviour
{
    private GameObject canvas;

    public GameController gameManager;
    public GameObject UICard;

    public float BottomMargin;
    public float CardGap;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.GetChild(0) is Transform parentCanvas && parentCanvas.GetChild(0) is Transform canvas)
        {
            this.canvas = canvas.gameObject;
            if (this.canvas.GetComponent<RectTransform>() is RectTransform rectTr)
            {
                float deckWidth = (gameManager.CurrentDeck.Count - 1) * CardGap;
                Vector3 left = new Vector3((-deckWidth) * 0.5f, 0, 0);
                Vector3 position = left;
                int i = 0;
                foreach (var card in gameManager.CurrentDeck)
                {
                    var uiCard = createCardUi(card, new Vector3(0, 0, 0), i++);
                    uiCard.transform.SetParent(this.canvas.transform);
                    uiCard.GetComponent<RectTransform>().anchoredPosition3D = position;
                    position += new Vector3(CardGap, 0, 0);
                }
            }
        }
    }

    GameObject createCardUi(BuildingDefinition building, Vector3 position, int instance)
    {
        var card = Instantiate(UICard, new Vector3(0, 0, 0), Quaternion.identity);
      //  card.GetComponent<CardUI>()?.Instantiate(building, () => { gameManager.SelectCard(instance); });

        return card;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
