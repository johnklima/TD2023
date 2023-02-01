using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// We are making an abstract class of Scriptable Object type
// Because this class itself is not going to be used to set any kind of values
// but will instead be used to create other movement calculation classes from
// This is essentially a base class to build up from
public abstract class FlockBehaviour : ScriptableObject
{

    // This function returns a vector3 by passing a FlockAgent script, List of Transforms and a Flock script
    // through it. It is intended to be used for other classes add agent instances into a transform list
    // so we can refer back to the list of agents to calculate different movement behaviours for 
    // Vector3 variables.
    public abstract Vector3 CalculateMove (FlockAgent agent, List<Transform> neighbourTrans, Flock flock);
}
