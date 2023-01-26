using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Flock/Behaviour/Composite")]

public class CompositeBehaviour : FlockBehaviour
{
    [SerializeField] FlockBehaviour[] behaviours; 
    [SerializeField] float[] importance;


    public override Vector3 CalculateMove (FlockAgent agent, List<Transform> neighbourTrans, Flock flock)
    {
        // setting up a error message, in the event that I end up adding more flock behaviours
        // and forget to change the number in importance
        if (importance.Length != behaviours.Length)
        {
            Debug.LogError("missing number of behaviours");
        }

        Vector3 move = Vector3.zero;

        // iterate through behaviours
        for (int i = 0; i < behaviours.Length; i++)
        {
            // make a new vector3 which takes in the returned value of all of the CalculateMove functions
            // from all of the behaviour scripts. This is multiplied by the float array, so each behaviour is
            // multiplied with a number.
            // multiplying the behaviours with a number, assigns them a value which determines how 
            // much emphasis/importance one behaviour should have over another
            Vector3 groupMovements = behaviours[i].CalculateMove(agent, neighbourTrans, flock) * importance[i];

            // if the new vector is not zero
            if (groupMovements != Vector3.zero)
            {
                // then check if new vector has movement greater than the value of the float array
                if (groupMovements.sqrMagnitude > importance[i] * importance[i])
                {
                    // true, normalise the movements and multiply the vector by the float array
                    groupMovements.Normalize();
                    groupMovements *= importance[i];
                }
            }
            
            move += groupMovements;
        }

        return move;

    }
}
