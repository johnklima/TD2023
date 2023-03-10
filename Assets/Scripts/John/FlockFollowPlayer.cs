using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockFollowPlayer : MonoBehaviour
{
    public bool doFollow = false;
    public Transform player;
    public float height = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (doFollow)
        {
            Vector3 pos = new Vector3(player.position.x, height, player.position.z);
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
        }

    }
}
