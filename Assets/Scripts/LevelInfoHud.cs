using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelInfoHud : MonoBehaviour
{
    public GameController gameController;

    public TMP_Text Username;
    public TMP_Text Level;
    public TMP_Text Stage;
    public TMP_Text Requirements;
    public TMP_Text Bombs;

    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        var levelInfo = gameController.LevelInfo;
        Level.text = $"Level: {levelInfo.Name}";
        Username.text = $"User: {gameController?.Player?.Name ?? "Joe Doe"}";
        Stage.text = $"Stage: 0";
        //Bombs.text = $"Bombs Left: {levelInfo.Bombs}";

        var reqs = string.Join('\n', levelInfo.Requirements.Select(req => $"{req.Name}: {req.Min} < x < {req.Max}"));
        Requirements.text = reqs;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
