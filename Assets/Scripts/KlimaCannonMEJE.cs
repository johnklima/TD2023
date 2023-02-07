using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class KlimaCannonMEJE : MonoBehaviour
{
    [Header("Insert Here")]
    [SerializeField] private RectTransform ChargingUI;
    private Image sprite;
    
    [SerializeField] private Transform ammo;
    private Coroutine FireCannon;

    private float aimAmount, aimCap = 10, cooldown = 0.5f;
    private bool firing = false;
    
    [Header("John's Stuff Below - Ignore")]
    public float G = 9.8f;
    public Vector3 direction;

    public Transform cannon;
    public Vector3 end;

    public GravityME grav;  //get this from the ball being currently fired

    // Start is called before the first frame update
    private void Start()
    {
        cannon = transform;
        if(ChargingUI)
            sprite = ChargingUI.GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        while (cooldown < 0.5f)
        {
            FadeUI(true);
            
            cooldown += Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
            FireCannon = StartCoroutine(StartCharge());

        if (Input.GetKeyUp(KeyCode.Mouse0))
            FireCharge();
    }
    
    private IEnumerator StartCharge()
    {
        for(int i = 0; i < ammo.childCount; i++) //The list of balls
        {
            grav = ammo.GetChild(i).GetComponent<GravityME>();
            if (!grav.inAir)
                break;
        }
        
        FadeUI(false);
            
        aimAmount = 0;
        firing = true;
        
        float time = 0;
        while (time < 2)
        {
            aimAmount += (Time.deltaTime * aimCap);
            ChargingUI.sizeDelta += new Vector2(0f, 50f * Time.deltaTime);
            ChargingUI.anchoredPosition += new Vector2(0f, 25f * Time.deltaTime);
            //Debug.DrawRay(start.position, aimPos + aimPos, Color.green, time);
            yield return null;
            time += Time.deltaTime;
        }
    }

    private void FireCharge()
    {
        if (!firing)
            return;
        
        if (FireCannon != null)
            StopCoroutine(FireCannon);
        
        end = transform.position - new Vector3(0f, 0.1f, 0f) + transform.forward * aimAmount;
        
        grav.gameObject.SetActive(true);
        grav.inAir = true;
        grav.transform.position = transform.position + new Vector3(0f, 0.1f, 0f);
        
        direction = end - cannon.position;
        grav.impulse = fire(cannon.position, end, 30.0f);

        firing = false;
        cooldown = 0;
    }
    
    public Vector3 fire(Vector3 startPoint, Vector3 endPoint, float desiredAngle)
    {
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

        //Debug.Log(Vo.ToString());
        //multiply by calculated "powder charge"
        return angV;
    }

    private void FadeUI(bool fade)
    {
        if (fade)
        {
            Color alpha = sprite.color;
            alpha -= alpha;
            alpha.a = 1f;
            sprite.color -= alpha * Time.deltaTime * 2;
        }
        else
        {
            Color alpha = sprite.color;
            alpha.a = 1f;
            sprite.color = alpha;
            
            ChargingUI.sizeDelta = new Vector2(100, 0);
            ChargingUI.anchoredPosition = new Vector2(0, -50);
        }
    }
}