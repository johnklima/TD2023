using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Segments3D : MonoBehaviour
{
    public Vector3 Apos = new Vector3(0, 0, 0);
    public Vector3 Bpos = new Vector3(0, 0, 0);

    public float length = 0;

    public _Segments3D parent = null;
    public _Segments3D child = null;

    public void updateSegmentsAndChildren()
    {
        if (child) child.updateSegmentsAndChildren();
    }

    public void updateSegment()
    {
        if (parent)
        {
            Apos = parent.Bpos;
            transform.position = Apos;
        }
        else
        {
            Apos = transform.position;
        }

        calculateBpos();
    }

    void calculateBpos()
    {
        Bpos = Apos + transform.forward * length;
    }

    public void pointAt(Vector3 target)
    {
        transform.LookAt(target);
    }

    public void drag(Vector3 target)
    {
        pointAt(target);
        transform.position = target - transform.forward * length;

        if (parent)
        {
            parent.drag(transform.position);
        }
        {
            
        }
    }

    public void reach(Vector3 target)
    {
        drag(target);
        updateSegment();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
