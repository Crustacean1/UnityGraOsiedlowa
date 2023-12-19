//using UnityEngine;
//using UnityEngine.AI;

//public class PedestrianMovement : MonoBehaviour
//{
//    private NavMeshAgent agent;

//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();
//    }

//    void Update()
//    {
//        if (Input.GetMouseButtonDown(2))
//        {
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//            RaycastHit hit;

//            if (Physics.Raycast(ray, out hit))
//            {
//                agent.SetDestination(hit.point);
//            }
//        }
//    }
//}

using UnityEngine;
using UnityEngine.AI;

public class PedestrianMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetRandomDestination();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SetRandomDestination();
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomPoint = RandomNavmeshLocation(10f); // Zmieñ 10f na odleg³oœæ, na jakiej chcesz znaleŸæ nowy punkt
        agent.SetDestination(randomPoint);
    }

    Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, 1);
        return hit.position;
    }
}
