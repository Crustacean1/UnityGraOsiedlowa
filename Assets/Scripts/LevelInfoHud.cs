using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelInfoHud : MonoBehaviour
{
    public GameController gameController;

    public TMP_Text Level;
    public TMP_Text Username;
    public TMP_Text Bombs;

    // Start is called before the first frame update
    void Start()
    {
        /*
        var levelInfo = gameController.LevelInfo;
        Level.text = $"Level: {levelInfo.Name}";
        Username.text = $"User: {gameController.Player.Name}";
        Bombs.text = $"Bombs Left: {levelInfo.Bombs}";
        */
    }

    // Update is called once per frame
    void Update()
    {

    }
}
