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

    public GameObject RequirementsPanel;
    public GameObject Requirement;

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
        Stage.text = $"Stage: {gameController?.LevelInfo.CurrentTurn}";

        foreach (Transform child in RequirementsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        var delta = new Vector3(0, -30, 0);
        var i = -1;

        foreach (var requirement in levelInfo.Requirements)
        {
            var requirementObject = Instantiate(Requirement, RequirementsPanel.transform);
            requirementObject.GetComponent<RectTransform>().sizeDelta = new Vector3(400, 30);
            requirementObject.transform.localPosition += delta * i++;

            if (requirementObject.GetComponent<TMP_Text>() is TMP_Text requirementLabel)
            {
                requirementLabel.text = $"{requirement.Name}:\t{requirement.Min} < {requirement.Current} < {requirement.Max}";
                if (requirement.Min > requirement.Current)
                {
                    requirementLabel.color = Color.red;
                }
                else if (requirement.Max < requirement.Current)
                {
                    requirementLabel.color = Color.yellow;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
