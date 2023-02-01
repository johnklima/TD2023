using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    // this script needs to be attacked to a game object. Can be empty
    
    // This Flock class is responsible for instansiating our scene with the agent prefabs
    // and holding the behaviours of the agents


    [SerializeField] FlockAgent agentPrefab;

    List<FlockAgent> agentList = new List<FlockAgent>();

    [SerializeField] FlockBehaviour behaviour;

    [SerializeField] int startingAmount = 30;
    // agent density is the distance that agents must be apart within the flock
    const float agentDensity = 0.08f;

    Collider[] otherColliders;

    [Range(1f, 10f)] // the radius distance that agents should keep from neighbouring colliders
    [SerializeField] float neighbourRadius = 1.5f;
     // radius for the avoidance between agents and neighbouring colliders
    [SerializeField] float avoidanceRadius = 0.5f;
    public float AvoidanceRadius {get{return avoidanceRadius;}}


    // Start is called before the first frame update
    void Start()
    {
        /*
        In the start method we instansiate a list full of new agent prefabs.
        the prefabs are instansiated inside of a sphere with radius 1. 
        the amount that is spawned is the startingAmount, 
        */
        for (int i = 0; i < startingAmount; i++)
        {
            FlockAgent newAgent = Instantiate(
            agentPrefab, Random.insideUnitSphere * startingAmount * agentDensity,
            Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), transform
            );
            // i might want to change the rotation values later
            
            // give each instanced agent a name and add them to the list
            newAgent.name = "Agent" + i;
            agentList.Add(newAgent);
        }
         
    }

    // Update on this script is where we ultimately make movement happen
    // based on our other scripts.
    void Update()
    {
        /*
        for each gameobject with a FlockAgent script in the list, add that agent to a new list.
        the new list holds Transforms of the spawned agent gameObject
        */

        foreach (FlockAgent agent in agentList)
        {
            List<Transform> otherTransform = GetNearbyObjects(agent);
            /*
            CalculateMove is a public method from the FlockBehaviour script.
            We are creating a new instance here, which will take in information from our behaviour script,
            calculating the agent's relation to other agents, within this flock gameobject
            */
            Vector3 move = behaviour.CalculateMove(agent, otherTransform, this);

            // make our new vector3 that we made from the attached FlockBehaviour,
            // and make it move according to those rules
            agent.Move(move);
            
        }
    }

    // transform list method that checks for agent objects that are nearby other agents inside of the list of Transforms
    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        // new empty list of transforms
        List<Transform> otherTransform = new  List<Transform>();
        
        // collider array which holds all overlapping colliders within the neighbour Radius
        otherColliders = Physics.OverlapSphere(agent.transform.position, neighbourRadius);
        foreach (Collider collider in otherColliders)
        {
            // if each checked collider is not overlapping with itself
            // add that colliders transform to transform list
            if (collider != agent.AgentCollider)
            {
                otherTransform.Add(collider.transform);
            }
        }
        // return transform list filled with the transforms of all the agent prefabs we instanced
        // we need the transforms because they need to pass through our FlockBehaviour scripts.
        // That way we can tell each instanced transform how to behave in relation to other 
        // transforms within its list.
        return otherTransform;
    }
}
