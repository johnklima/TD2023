using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffset : MonoBehaviour
{

    [SerializeField] Vector3 beginPos;
    [SerializeField] Vector3 finalPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = beginPos;
    }
    void LateUpdate()
    {
        transform.localPosition = finalPos;
    }
}
