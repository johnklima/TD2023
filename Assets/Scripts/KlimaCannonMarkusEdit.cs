using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlimaCannonMarkusEdit : MonoBehaviour
{
    private int ammo = 8, damage = 1;
    private Vector3 scale = new Vector3(0.29f, 0.29f, 0.29f);
    private float aimCap = 10;

    public float G = 9.8f;
    public Vector3 direction;

    public Transform start;
    public Vector3 end;

    public Gravity grav;  //get this from the ball beign currently fired

   
    // Start is called before the first frame update
    private void Start()
    {
        start = transform;
        grav = GetComponent<Gravity>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) )
            StartCoroutine(StartCharge());

        if (Input.GetKeyUp(KeyCode.Mouse0) )
            FireCharge();
    }
    
    private IEnumerator StartCharge()
    {
        end = transform.position - new Vector3(0, 1, 0);
        
        float time = 0;
        while (time < 2)
        {
            end += transform.forward * (Time.deltaTime * aimCap);
            //Debug.DrawRay(start.position, aimPos + aimPos, Color.green, time);
            yield return null;
            time += Time.deltaTime;
        }
        
        /*Ray ray = Camera.main.ScreenPointToRay(aimPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 40))
            end = hit.point;
        
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 40))*/
    }

    private void FireCharge()
    {

        Transform balls = transform.GetChild(0); //the list of balls
        for(int i = 0; i < balls.childCount; i++)
        {
            grav = balls.GetChild(i).GetComponent<Gravity>();
            if (!grav.isInAir)
                break;
        }
        
        StopCoroutine(StartCharge());
        
        transform.position += Vector3.up;
            
        grav.impulse = fire(start.position, end, 30.0f);
    }

    public Vector3 fire(Vector3 startPoint, Vector3 endPoint, float desiredAngle)
    {
        direction = endPoint - startPoint;
        
        Vector3 t = endPoint;
        Vector3 c = startPoint;

        //get a flat distance
        t.y = c.y = 0;
        float flatdistance = Vector3.Distance(t, c);

        Vector3 P1, P2;
        P1 = P2 = endPoint - startPoint;

        //get just the flat direction without y component
        P1.y = 0;
        P1.Normalize();

        //unitize the vector
        P2.Normalize();

        //get angle in rads, this is a minimum launch angle (if down to up)
        float angle = Mathf.Acos(Vector3.Dot(P1, P2));

        //any angle less than 45 is 45 (optimal angle)
        //any angle greater is the angle, plus HALF the angle of the vector to y up.
        //with balistics the angle ALWAYS has to be greater than the direct angle from
        //point to point. so we add half of this angle to 90, splitting the difference

        if (angle < 0.785398163f)
            angle = 0.785398163f;
        else
            angle += Mathf.Acos(Vector3.Dot(P1, Vector3.up)) * 0.5f;


        //if it is too close to pure vertical make it less than pure vertical
        if (angle > Mathf.PI / 2 - 0.05f)
            angle = Mathf.PI / 2 - 0.05f;

        //if we are going from up to down, just use appx 60 degs
        if (startPoint.y > endPoint.y)
            angle = 0.985398163f;

        //if we are asked to use a specific angle, this is our angle..
        //if (desiredAngle != 0)
        //{
        //    angle = Mathf.Deg2Rad * desiredAngle;
        //}

        float Y = endPoint.y - startPoint.y;

        //are we firing down a hill?
        // if (endPoint.y < startPoint.y && false)
        // {
        //     angle *= -0.5f;
        // }

        // perform the trajectory calculation to arrive at the gun powder charge aka 
        // target velocity for the distance we desire, based on launch angle
        float rng = 0;
        float Vo = 0;
        float trydistance = flatdistance;
        int iters = 0;

        //now iterate until we find the correct proposed distance to land on spot.
        //this method is based on the idea of calculating the time to peak, and then
        //peak to landing at the height differential. we can then extract an XY distance 
        //achieved regardless of height differential. by iterating with a binary heuristic
        //we increase or decrease the initial velocity until we acheive our XY distance

        float f = (Mathf.Sin(angle * 2.0f));
        if (f <= 0)
            f = 1.0f;      //just for safety (though this should never be)

        while (Mathf.Abs(rng - flatdistance) > 0.001f && iters < 64)
        {
            //make sure we dont squirt on a negative number. we can IGNORE that result
            if (trydistance > 0)
            {
                //create an initial force 
                float sqrtcheck = (trydistance * G) / f;
                if (sqrtcheck > 0)
                    Vo = Mathf.Sqrt(sqrtcheck);
                else
                    Debug.Log("sqrtcheck < 0 initial force");

                //find the vector of it in our trajectory planar space
                float Vy = Vo * Mathf.Sin(angle);
                float Vx = Vo * Mathf.Cos(angle);

                //get a height and thus time to peak
                float H = -Y + (Vy * Vy) / (2 * G);
                float upt = Vy / G;                      // time to max height

                //if again we squirt on a neg, but it is because our angle and force
                //are too accute. note: we handle up and down trajectory differently
                if (2 * H / G < 0)
                {
                    if (endPoint.y < startPoint.y)
                        rng = flatdistance;       //if going down we are done
                    else
                        rng = 0;       //if up we are not
                }
                else
                {
                    sqrtcheck = 2 * H / G;
                    if (sqrtcheck > 0)
                    {
                        float dnt = Mathf.Sqrt(sqrtcheck);           // time from max height to impact
                        rng = Vx * (upt + dnt);
                    }
                    else
                        Debug.Log("sqrtcheck < 0 H / Gravity");
                }

                if (rng > flatdistance)
                    trydistance -= (rng - flatdistance) / 2;		//using a binary zero-in, it takes about 8 iterations to arrive at target
                else if (rng < flatdistance)
                    trydistance += (flatdistance - rng) / 2;


                //Debug.Log("ITERS = " + iters);
            }
            else
                iters = 64;
            iters++;
        }

        Vector3 angV = new Vector3(direction.x, 0, direction.z);
        
        angV.Normalize();
        Vector3 side = Vector3.Cross(angV, Vector3.up);
        side.Normalize();
       
        //we need to rotate that by our actual launch angle
        angV = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, side) * angV;

        angV *= Vo;

        Debug.Log(Vo.ToString());
        //multiply by calculated "powder charge"
        return angV;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CannonTarget")  //gonna need some "Or's" here, LayerMask? 
        {
            Debug.Log("ball hit " + other.name);
            grav.reset();
            inAir = false;
            transform.localPosition = Vector3.zero;
        }
    }
}