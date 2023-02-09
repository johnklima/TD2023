using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSmoothShift : MonoBehaviour
{

    [SerializeField] Vector3 beginShift;
    [SerializeField] Vector3 endShift;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = beginShift;
    }
    private void LateUpdate()
    {
        transform.localPosition = endShift;
    }
}
