using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the behavior of pedestrian NPCs.
/// </summary>
public class Pedestrian : MonoBehaviour
{
    /// <summary>
    /// Reference to the NavMeshAgent component.
    /// </summary>
    public UnityEngine.AI.NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UnityEngine.Debug.Log($"So close: {(transform.position - agent.destination).magnitude }");
        if ((transform.position - agent.destination).magnitude < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
