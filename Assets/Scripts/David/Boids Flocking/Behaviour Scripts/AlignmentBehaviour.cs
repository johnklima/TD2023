using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Flock/Behaviour/Alignment")]

public class AlignmentBehaviour : FlockBehaviour
{
    public override Vector3 CalculateMove (FlockAgent agent, List<Transform> neighbourTrans, Flock flock)
    {
        // if there are no neighbours in our list, continue forward
        if (neighbourTrans.Count == 0)
        {
            return agent.transform.forward;
        }

        Vector3 alignmentMove = Vector3.zero;
        foreach (Transform trans in neighbourTrans)
        {
            alignmentMove += trans.transform.forward;
        }
        alignmentMove/= neighbourTrans.Count;

        return alignmentMove;
    }
}
