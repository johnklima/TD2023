using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class _IKSnake : MonoBehaviour
{
    private int childCount = 0;

    private bool isDragging;
    public _Segments3D[] segments;
    public Transform target = null;
    public int children;
    private _Segments3D lastSegment = null;
    private _Segments3D firstSegment = null;
    
    

    private void Awake()
    {
        // childCount = transform.childCount;
        // segments = new Segments3D[childCount];
        // int i = 0;
        // foreach (Segments3D child in segments)
        // {
        //     for (i = 0; i < childCount; i++)
        //     {
        //         int childsChild = i + 1;
        //         segments[i].child = transform.GetChild().GetChild(childsChild);
        //         
        //     }
        //     
        // }
        
        firstSegment = segments[0];
        lastSegment = segments[childCount - 1];
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        // (children in transform.parent)
        // {
        //     
        // }
    }
}
