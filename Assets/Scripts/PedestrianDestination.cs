//using UnityEngine;
//using UnityEngine.AI;

//public class PedestrianDestination : MonoBehaviour
//{
//    public Transform destination;

//    private NavMeshAgent agent;

//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        SetDestination();
//    }

//    void SetDestination()
//    {
//        if (destination != null)
//        {
//            agent.SetDestination(destination.position);
//        }
//        else
//        {
//            Debug.LogWarning("Nie przypisano miejsca docelowego!");
//        }
//    }
//}

using UnityEngine;
using UnityEngine.AI;

public class PedestrianDestination : MonoBehaviour
{
    public Transform destination;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (destination == null)
        {
            Debug.LogWarning("Nie przypisano miejsca docelowego!");
            return;
        }

        // Ustaw pocz¹tkow¹ pozycjê jako miejsce docelowe
        destination.position = transform.position;
        SetDestination();
    }

    void SetDestination()
    {
        if (destination != null)
        {
            agent.SetDestination(destination.position);
        }
        else
        {
            Debug.LogWarning("Nie przypisano miejsca docelowego!");
        }
    }
}
