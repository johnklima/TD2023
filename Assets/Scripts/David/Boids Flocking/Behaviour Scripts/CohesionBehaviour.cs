using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Flock/Behaviour/Cohesion")]

/*
This cohesion behaviour intends to group agents together
*/

public class CohesionBehaviour : FlockBehaviour
{

    public override Vector3 CalculateMove (FlockAgent agent, List<Transform> neighbourTrans, Flock flock)
    {
        // if there are no neighbours in our list, then there is no course adjustment
        if (neighbourTrans.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 cohesionMove = Vector3.zero;
        foreach (Transform trans in neighbourTrans)
        {
            cohesionMove += trans.position;
        }
        cohesionMove/= neighbourTrans.Count;

        cohesionMove -= agent.transform.position;
        return cohesionMove;
    }
}
