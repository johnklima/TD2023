using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Flock/Behaviour/Avoidance")]

public class AvoidanceBehaviour : FlockBehaviour
{
    public override Vector3 CalculateMove (FlockAgent agent, List<Transform> neighbourTrans, Flock flock)
    {
        // if there are no neighbours in our list, then there is no course adjustment
        if (neighbourTrans.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 avoidanceMove = Vector3.zero;
        int numOfAvoid = 0;
        foreach (Transform trans in neighbourTrans)
        {
            if (Vector3.Magnitude(trans.position - agent.transform.position) < flock.AvoidanceRadius)
            {
                numOfAvoid++;
                avoidanceMove += (agent.transform.position - trans.position);
            }
            avoidanceMove += trans.position;
        }
        if (numOfAvoid > 0)
        {
            avoidanceMove /= numOfAvoid;
        }

        return avoidanceMove;
    }
}
