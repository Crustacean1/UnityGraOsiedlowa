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
    public GameManager gameManager;
    public GameObject UICard;

    public float BottomMargin;
    public float CardGap;

    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.transform.GetChild(0) is Transform canvas)
        {
            this.canvas = canvas.gameObject;
            if(this.canvas.GetComponent<RectTransform>() is RectTransform rectTr)
            {
                float deckWidth = (gameManager.CurrentDeck.Count - 1) * CardGap;
                Vector3 left = new Vector3((rectTr.rect.width - deckWidth) * 0.5f, BottomMargin, 0);
                Vector3 position = left;
                int i = 0;
                foreach(var cardId in gameManager.CurrentDeck)
                {
                    var card = createCardUi(cardId, position, i++);
                    card.transform.SetParent(this.canvas.transform);
                    position += new Vector3(CardGap, 0, 0);
                }
            }
        }
    }

    GameObject createCardUi(int id, Vector3 position, int instance)
    {
	var card = Instantiate(UICard, position, Quaternion.identity);
        card.GetComponent<CardUI>()?.Instantiate(gameManager.CardPrefabs[id], () => { gameManager.CurrentCard = gameManager.CardPrefabs[gameManager.CurrentDeck[instance]]; }) ;
        return card;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
