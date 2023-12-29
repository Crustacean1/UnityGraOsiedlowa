using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controls the user interface for the ending summary.
/// </summary>
public class EndingUi : MonoBehaviour
{
    /// <summary>
    /// Level informations.
    /// </summary>
    private LevelDefinition levelInfo;

    /// <summary>
    /// Player.
    /// </summary>
    private Player player;

    /// <summary>
    /// The text field for the summary information.
    /// </summary>
    public TMP_Text Summary;

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void OnExit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Restarts the game.
    /// </summary>
    public void OnRestart()
    {
        if (FindObjectsOfType<GodScript>().SingleOrDefault() is GodScript godScript)
        {
            var currentLevel = godScript.CurrentLevel.Name;
            var newLevel = godScript.LevelDefinitions.FindIndex(l => l.Name == currentLevel);
            godScript.StartGame(godScript.Player, newLevel);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<GodScript>().SingleOrDefault() is GodScript godScript)
        {
            levelInfo = godScript.CurrentLevel;
            player = godScript.Player;
        }

        var fulfilled = levelInfo.Requirements.Where(r => r.Min <= r.Current && r.Current <= r.Max).Count();
        var unfulfilled = levelInfo.Requirements.Where(r => r.Min > r.Current && r.Current > r.Max).Count();

        var victory = fulfilled > unfulfilled ? "You Won" : "You Lost";

        Summary.text = string.Join('\n', victory,
            $"Player: {player.Name}",
            $"Level: {levelInfo.Name}",
            $"Satisfied Requirements: {fulfilled}",
            $"Unsatisfied Requirements: {unfulfilled}"
            );
    }

    // Update is called once per frame
    void Update()
    {

    }
}
