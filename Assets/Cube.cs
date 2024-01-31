using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    
    int cubevalue;

    // Start is called before the first frame update
    void Start()
    {


        cubevalue = 1;

    }

    // Update is called once per frame
    void Update()
    {
        int curint = myMethod();
        curint += (int) 2.7f;
        curint += 10;

        Debug.Log("Value = " + curint);

        myMethod();
    }

    int myMethod()
    {
        cubevalue++;

        return cubevalue;
    }
}
