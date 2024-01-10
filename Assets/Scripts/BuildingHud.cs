using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingHud : MonoBehaviour
{
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Focus(BuildingDefinition definition)
    {
        System.Func<string, string> format = (string key) =>
        {
            if (definition.Properties.ContainsKey(key))
            {
                return definition.Properties[key][definition.Level].ToString();
            }
            else
            {
                return "N/A";
            }
        };

        var name = $"Name : {definition.Name} (Level {definition.Level}";
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
