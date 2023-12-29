using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for displaying building information in the HUD.
/// </summary>
public class BuildingHud : MonoBehaviour
{
    /// <summary>
    /// Text component used to display building information.
    /// </summary>
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// Focuses on displaying information for the provided building definition.
    /// </summary>
    /// <param name="definition">The definition of the building to display information for.</param>
    public void Focus(BuildingDefinition definition)
    {
        System.Func<string, string> format = (string key) =>
        {
            if (definition.Properties.ContainsKey(key))
            {
                return definition.Properties[key].ToString();
            }
            else
            {
                return "N/A";
            }
        };

        var name = $"Name : {definition.Name}";
        var population = $"Population: {format("Population")}";
        var condignations = $"Floors: {format("Floors")}";
        var area = $"Area: {format("Area")}";

        UnityEngine.Debug.Log("I am so done");

        text.SetText(string.Join('\n', "Building Info", name, population, condignations, area));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
